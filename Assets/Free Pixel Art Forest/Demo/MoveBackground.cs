using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float speed;
    public float parallaxEffect;
    public Transform playerTransform;
    public string playerTag = "Player"; // 플레이어 오브젝트의 태그
    private float lastPlayerX;

    void Start()
    {
        // 플레이어 오브젝트 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            lastPlayerX = playerTransform.position.x;
        }
        lastPlayerX = playerTransform.position.x;
    }

    void Update()
    {
        // 플레이어 오브젝트가 존재하는지 확인
        if (playerTransform != null)
        {
            float playerDeltaX = playerTransform.position.x - lastPlayerX;
            float bgDeltaX = playerDeltaX * parallaxEffect;
            transform.position += Vector3.right * (bgDeltaX + speed * Time.deltaTime);
            lastPlayerX = playerTransform.position.x;
        }
    }
}