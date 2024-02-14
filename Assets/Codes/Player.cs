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

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isAuto;
    private bool enableControll;
    private Vector3 targetPos;

    // Start is called before the first frame update

    public void OutDoor(Vector3 pos, Action complateAction)
    {
        targetPos = pos;
        isAuto = true;

        StartCoroutine(Wait(delegate() {
            enableControll = false;
            anim.SetFloat("Speed", new Vector3(0f, 1f, 0f).magnitude);
            StartCoroutine(MoveTransformPosition(transform, transform.position, new Vector2(transform.position.x, transform.position.y + 1f), 1f, fadeCurve, delegate()
            {
                if (complateAction != null) complateAction();
            }));
            StartCoroutine(Fade(GetComponent<SpriteRenderer>(), 1f, 0f, 1f, fadeCurve, null));
        }));
    }

    private IEnumerator Wait(Action action)
    {
        yield return new WaitForSeconds(2f);
        if (action != null) action();
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isAuto = false;
        enableControll = true;
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
                inputVec = (targetPos - transform.position).normalized;
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
                sprite.flipX = inputVec.x < 0;
            }
        }
        
    }

    void FixedUpdate()
    {
        Vector2 nextVec = rigid.position + inputVec * speed * Time.fixedDeltaTime;
        transform.position = new Vector3(nextVec.x, nextVec.y, sprite.bounds.min.y);
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
