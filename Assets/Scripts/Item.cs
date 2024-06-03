using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    public float destroyDelay = 3f; // 아이템 자동 파괴 지연 시간
    private void Start()
    {
        // 일정 시간 후에 아이템 파괴
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어와 충돌한 경우
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 플레이어 컨트롤러의 ApplyItemEffect 메서드 호출
                playerController.ApplyItemEffect();
            }

            // 아이템 파괴
            Destroy(gameObject);
        }
    }
}
