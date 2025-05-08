using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
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
        //jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump")){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }

        //stop speed
        // if (Input.GetButtonUp("Horizontal"))
        // {
        //     rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        // }
        float deceleration = 0.9f;
        if (Input.GetAxisRaw("Horizontal") == 0){
            rigid.velocity = new Vector2(rigid.velocity.x * deceleration, rigid.velocity.y);
        }

        //direction sprite
        if (Input.GetAxisRaw("Horizontal") != 0){
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3){
            anim.SetBool("isWalk", false);
        }
        else{
            anim.SetBool("isWalk", true);
        }
    }

    void FixedUpdate()
    {
        //move speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //max speed
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * -1)
            rigid.velocity = new Vector2(maxSpeed * -1, rigid.velocity.y);

        //landing platform
        if(rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null){
                if(rayHit.distance < 1f){
                    anim.SetBool("isJump", false);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
