using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 inputVec;
    public float speed;
    public RuntimeAnimatorController[] animCon;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anim;

    // Start is called before the first frame update

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.runtimeAnimatorController = animCon[0];
    }

    // 毎フレーム呼ばれる基本処理を書くところ
    void Update()
    {
        inputVec = variableJoystick.Direction;

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");

    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
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
