using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //stop speed
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        
        //direction sprite
        if(Input.GetButtonDown("Horizontal")){
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if(Mathf.Abs(rigid.velocity.x) < 0.3){
            anim.SetBool("isWalk", false);
        }
        else{
            anim.SetBool("isWalk", true);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed)
        rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed * -1)
        rigid.velocity = new Vector2(maxSpeed * -1, rigid.velocity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
