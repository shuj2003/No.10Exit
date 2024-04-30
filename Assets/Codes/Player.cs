using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Common
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 inputVec;
    public float speed;
    public RuntimeAnimatorController[] animCon;
    public AnimationCurve fadeCurve;
    public GameObject CameraFollow;

    private Collider2D coll;
    private Rigidbody2D rigid;
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
        rigid = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void InDoor(Vector3 pos, Action complateAction)
    {
        targetPos = pos;
        isAuto = true;
        coll.isTrigger = false;

        StartCoroutine(Wait(delegate() {
            enableControll = false;
            anim.SetFloat("Speed", new Vector3(0f, 1f, 0f).magnitude);
            StartCoroutine(MoveTransformPosition(transform, transform.position, new Vector2(transform.position.x, transform.position.y + 1f), 1f, fadeCurve, delegate()
            {
                if (complateAction != null) complateAction();
                coll.isTrigger = true;
            }));
            foreach (var sprite in sprites)
            {
                StartCoroutine(Fade(sprite, 1f, 0f, 1f, fadeCurve, null));
            }
        }));
    }

    public void OutDoor(Vector3 pos, Action complateAction)
    {
        coll.isTrigger = true;
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
            anim.SetFloat("Speed", new Vector3(0f, 1f, 0f).magnitude);
            StartCoroutine(MoveTransformPosition(transform, posF, pos, 1f, fadeCurve, delegate ()
            {
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
                if(hoko.magnitude < 0.1f)
                {
                    inputVec = Vector3.zero;
                }
                else
                {
                    inputVec = hoko.normalized;
                }
            }
        }

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }

    void LateUpdate()
    {
        if (enableControll)
        {
            anim.SetFloat("Speed", inputVec.magnitude);

            if (inputVec.x != 0)
            {
                transform.localScale = new Vector2(inputVec.x < 0 ? -1f : 1f, 1f);
            }
        }
        
    }

    void FixedUpdate()
    {
        Vector2 nextVec = rigid.position + inputVec * speed * Time.fixedDeltaTime;
        transform.position = new Vector2(nextVec.x, nextVec.y);
        rigid.MovePosition(nextVec);

        float len = GameManager.instance.man.transform.position.x - GameManager.instance.player.transform.position.x;
        float len2 = len * len;
        float max2 = 10f * 10f;
        if (len2 < max2)
        {
            AudioManager.instance.bgmVolume = (1f - len2 / max2) * 0.2f;
        }
        else
        {
            AudioManager.instance.bgmVolume = 0f;
        }

    }


}
