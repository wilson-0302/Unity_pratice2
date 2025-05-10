using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
   public int maxHP = 100; // 최대 체력
    private int currentHP;  // 현재 체력

    void Start()
    {
        currentHP = maxHP; // 시작 시 체력 초기화
    }

    // 데미지를 받는 함수
    public void TakeDamage(int damage)
    {
        currentHP -= damage; // 체력 차감
        Debug.Log("몬스터 피해! 현재 HP: " + currentHP);

        // 체력이 0 이하이면 사망 처리
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 사망 시 호출
    void Die()
    {
        Debug.Log("몬스터 사망");
        Destroy(gameObject); // 게임 오브젝트 삭제
    }
}
