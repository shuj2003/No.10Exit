using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Common
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 inputVec;
    public float speed;
    public RuntimeAnimatorController[] animCon;
    public AnimationCurve fadeCurve;
    public GameObject CameraFollow;
    public GameObject headPos;
    public Action DeadAction;

    private Collider2D coll;
    private SpriteRenderer[] sprites;
    private Animator anim;
    private bool isAuto;
    private bool enableControll;
    private Vector3 targetPos;

    // Start is called before the first frame update

    private void Start()
    {
        isAuto = true;
        enableControll = false;
        foreach(var sprite in sprites)
        {
            Color color = sprite.color;
            sprite.color = new Color(color.r, color.g, color.b, 0f);
        }
        coll.isTrigger = true;
    }

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void AutoMove(Vector3 pos)
    {
        targetPos = pos;
        isAuto = true;
        coll.isTrigger = true;
    }

    public void InDoor(Vector3 pos, Action complateAction)
    {
        targetPos = pos;
        isAuto = true;
        coll.isTrigger = true;

        StartCoroutine(Wait(delegate() {

            //after 2 sec 
            enableControll = false;
            anim.SetFloat("Speed", 1f);
            AudioManager.instance.PlaySfxOnlyOne(AudioManager.Sfx.Run);

            StartCoroutine(MoveTransformPosition(transform, transform.position, new Vector2(transform.position.x, transform.position.y + 1f), 1f, fadeCurve, delegate()
            {

                anim.SetFloat("Speed", 0f);
                AudioManager.instance.StopSfxOnlyOne(AudioManager.Sfx.Run);

                if (complateAction != null) complateAction();

            }));

            foreach (var sprite in sprites)
            {
                StartCoroutine(Fade(sprite, 1f, 0f, 1f, fadeCurve, null));
            }

        }));
    }

    public void OutDoor(Vector3 pos, Action complateAction)
    {
        Vector3 posF = pos + new Vector3(0f, 1f, 0f);
        transform.position = posF;
        isAuto = true;
        enableControll = false;

        foreach (var sprite in sprites)
        {
            Color color = sprite.color;
            sprite.color = new Color(color.r, color.g, color.b, 0f);
        }  

        StartCoroutine(Wait(delegate () {

            //after 2 sec
            anim.SetFloat("Speed", 1f);
            AudioManager.instance.PlaySfxOnlyOne(AudioManager.Sfx.Run);

            StartCoroutine(MoveTransformPosition(transform, posF, pos, 1f, fadeCurve, delegate ()
            {

                anim.SetFloat("Speed", 0f);
                AudioManager.instance.StopSfxOnlyOne(AudioManager.Sfx.Run);

                if (complateAction != null) complateAction();

                enableControll = true;
                isAuto = false;
                coll.isTrigger = false;

            }));

            foreach (var sprite in sprites)
            {
                StartCoroutine(Fade(sprite, 0f, 1f, 1f, fadeCurve, null));
            }

        }));
    }

    private IEnumerator Wait(Action action)
    {
        yield return new WaitForSeconds(2f);
        if (action != null) action();
    }

    private void OnEnable()
    {
        anim.runtimeAnimatorController = animCon[0];
    }

    // 毎フレーム呼ばれる基本処理を書くところ
    void Update()
    {
        if (enableControll)
        {
            if (!isAuto)
            {
                inputVec = variableJoystick.Direction;
            }
            else
            {
                Vector3 hoko = targetPos - transform.position;
                inputVec = hoko.normalized;
            }
        }

    }

    void LateUpdate()
    {
        if (enableControll)
        {
            anim.SetFloat("Speed", inputVec.sqrMagnitude);

            if (inputVec.sqrMagnitude > 0.0f)
            {
                AudioManager.instance.PlaySfxOnlyOne(AudioManager.Sfx.Run);
            }
            else
            {
                AudioManager.instance.StopSfxOnlyOne(AudioManager.Sfx.Run);
            }

            if (inputVec.x != 0)
            {
                transform.localScale = new Vector2(inputVec.x < 0 ? -1f : 1f, 1f);
            }
        }

    }

    void FixedUpdate()
    {
        if (!isAuto)
        {
            Vector2 nextVec = new Vector2(transform.position.x, transform.position.y) + inputVec * speed * Time.fixedDeltaTime;
            transform.position = new Vector2(nextVec.x, nextVec.y);
        }
        else
        {
            Vector3 hoko = targetPos - transform.position;
            if (hoko.sqrMagnitude < speed * Time.fixedDeltaTime * speed * Time.fixedDeltaTime)
            {
                transform.position = targetPos;
            }
            else
            {
                Vector2 nextVec = new Vector2(transform.position.x, transform.position.y) + inputVec * speed * Time.fixedDeltaTime;
                transform.position = new Vector2(nextVec.x, nextVec.y);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Head"))
        {
            anim.SetTrigger("Dead");
            if (DeadAction != null) DeadAction();
        }

    }


}
