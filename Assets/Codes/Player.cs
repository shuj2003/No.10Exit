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
    private BoxCollider2D collision;

    // Start is called before the first frame update

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collision = GetComponent<BoxCollider2D>();
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
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }

    private void OnTriggerStay2D(Collider2D collision2) 
    {
        if (!collision2.CompareTag("Furniture") || collision2.GetType() != typeof(BoxCollider2D))
            return;

        SpriteRenderer sprite2 = collision2.GetComponent<SpriteRenderer>();
        Transform transform2 = collision2.gameObject.transform;
        BoxCollider2D boxCollider2D = (BoxCollider2D)collision2;
        var bottom = transform.localPosition.y + collision.offset.y - collision.size.y;
        var bottom2 = transform2.localPosition.y + boxCollider2D.offset.y - boxCollider2D.size.y;

        if (bottom2 < bottom)
        {
            sprite2.sortingOrder = sprite.sortingOrder + 1;
        }
        else
        {
            sprite2.sortingOrder = sprite.sortingOrder - 1;
        }

    }


}
