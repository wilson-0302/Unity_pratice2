using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //최대 이동 속도
    public float maxSpeed;
    // 점프 힘 (위로 튕겨지는 힘의 크기)
    public float jumpPower;
    // Rigidbody2D 컴포넌트 저장용
    Rigidbody2D rigid;
    // SpriteRenderer 컴포넌트 저장용 (좌우 반전용)
    SpriteRenderer spriteRenderer;
    // Animator 컴포넌트 저장용 (애니메이션 전환용)
    Animator anim;

    //공격
    private bool isAttacking = false;
    public float attackDelay = 0.5f; // 공격 지속 시간
    private float attackTimer = 0f;

    // 공격 관련 변수
    public LayerMask enemyLayer;        // 어떤 레이어가 '적'인지 정의 (Inspector에서 Enemy 레이어 선택)
    public Transform attackPoint;       // 공격 중심점 (보통 손 또는 무기 위치에 빈 오브젝트 지정)
    public float attackRange = 1f;      // 공격 범위 (원형 범위의 반지름)
    public int attackDamage = 20;       // 공격 시 입히는 데미지

    // Animator 컴포넌트 저장용 (애니메이션 전환용)
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // 매 프레임마다 호출되는 함수 (입력 처리 및 애니메이션 상태 제어)
    void Update()
    {
        // 스페이스바나 점프 버튼을 누르면 점프 (단, 점프 중이 아닐 때만)
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }

        // 좌우 입력이 없을 경우 감속 처리
        float deceleration = 0.9f;
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x * deceleration, rigid.velocity.y);
        }

        // 방향에 따라 캐릭터 좌우 반전
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // 이동 속도에 따라 걷기 애니메이션 상태 제어
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true);
        }


        // 공격 입력 처리
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            anim.SetBool("isAttack", true);
            attackTimer = attackDelay;

            // 공격 범위 내 적 감지
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Monster>().TakeDamage(attackDamage);
            }
        }

        // 공격 애니메이션 상태 해제 타이머
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                anim.SetBool("isAttack", false);
            }
        }
    }

    // 물리 연산이 필요한 로직을 처리하는 함수 (FixedUpdate는 일정 시간 간격으로 호출됨)
    void FixedUpdate()
    {
        // 좌우 방향 입력 받아서 힘을 가해 캐릭터 이동
        float h = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);

        // 캐릭터의 수평 속도를 최대 속도로 제한
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * -1)
            rigid.velocity = new Vector2(maxSpeed * -1, rigid.velocity.y);

        // 점프 후 낙하 중일 때 땅에 착지했는지 확인
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1f)
                {
                    anim.SetBool("isJump", false);
                }
            }
        }
    }

    // 게임 시작 직후 1회 호출되는 함수 (현재는 비어있음)
    void Start()
    {

    }

    // 공격 범위 디버그용 시각화
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
