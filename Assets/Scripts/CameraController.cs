using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public string ringTag = "Ring";
    public float cameraZPosition = -10f; // 카메라의 Z축 위치
    public float grapplingYOffset = 2f; // 그래플링 중일 때 카메라의 Y축 오프셋
    public float minX = -10f; // 카메라의 최소 X 좌표
    public float maxX = 10f; // 카메라의 최대 X 좌표

    private GrapplingHook grapplingHook;
    private Transform targetTransform;

    private void Start()
    {
        grapplingHook = playerTransform.GetComponent<GrapplingHook>();
    }

    private void FixedUpdate()
    {
        // playerTransform이 유효한지 확인
        if (playerTransform == null)
        {
            return;
        }
        if (grapplingHook.isAttach)
        {
            // 충돌 중인 Ring을 찾음
            Collider2D[] colliders = Physics2D.OverlapCircleAll(grapplingHook.transform.position, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag(ringTag))
                {
                    targetTransform = collider.transform;
                    break;
                }
            }

            if (targetTransform != null)
            {
                // 카메라를 Ring의 위치로 이동
                Vector3 desiredPosition = targetTransform.position + offset;
                desiredPosition.y += grapplingYOffset; // 그래플링 중일 때 Y축 오프셋 적용
                desiredPosition.z = cameraZPosition; // 카메라의 Z축 위치 고정
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX); // X 좌표 제한
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
        else
        {
            // 그래플링 중이 아닐 때는 Player의 위치를 따라감
            Vector3 desiredPosition = playerTransform.position + offset;
            desiredPosition.z = cameraZPosition; // 카메라의 Z축 위치 고정
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX); // X 좌표 제한
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            targetTransform = null;
        }
    }
}