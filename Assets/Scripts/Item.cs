using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f; // 아이템 자동 비활성화 지연 시간

    private const string PLAYER_TAG = "Player";
    private const string ITEM_POOL_TAG = "Item"; // ObjectPooler에서 사용할 태그

    private void Start()
    {
        // Start 대신 OnEnable을 사용하여 오브젝트가 활성화될 때마다 코루틴을 실행합니다.
    }

    private void OnEnable()
    {
        // 오브젝트가 풀에서 나올 때(활성화될 때) 자동 비활성화 코루틴을 시작합니다.
        StartCoroutine(DeactivateAfterTime(lifetime));
    }

    private IEnumerator DeactivateAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 시간이 다 되면 오브젝트 풀로 반환합니다.
        ObjectPooler.Instance.ReturnToPool(ITEM_POOL_TAG, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyItemEffect();
            }

            // 코루틴을 중지하고 즉시 풀로 반환합니다.
            StopAllCoroutines();
            ObjectPooler.Instance.ReturnToPool(ITEM_POOL_TAG, gameObject);
        }
    }
}
