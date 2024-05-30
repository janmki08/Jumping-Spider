using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTongue : MonoBehaviour
{
    private Vector3 tongueEndPosition;
    public LineRenderer lineRenderer;
    public float tongueSpeed = 10f;
    public float tongueLength = 5f;
    public float tongueDuration = 1f;

    private bool isShooting = false;
    private float tongueTimer = 0f;
    private Vector3 tongueDirection;
    private float shootInterval = 2f; // 발사 간격
    private float shootTimer = 0f; // 발사 타이머

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootTongue();
        }
        if (isShooting)
        {
            tongueTimer += Time.deltaTime;

            if (tongueTimer <= tongueDuration)
            {
                tongueEndPosition = transform.position + tongueDirection * tongueLength * (tongueTimer / tongueDuration);
                lineRenderer.SetPosition(1, tongueEndPosition);
            }
            else
            {
                tongueEndPosition = transform.position;
                lineRenderer.SetPosition(1, tongueEndPosition);
                isShooting = false;
            }
        }
    }

    private void ShootTongue()
    {
        Debug.Log("혀");
        isShooting = true;
        tongueTimer = 0f;
        tongueDirection = Quaternion.Euler(0f, 0f, -30f) * Vector3.left;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isShooting && collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null && !playerController.IsInvincible)
            {
                Vector3 playerPosition = collision.transform.position;
                if (Vector3.Distance(tongueEndPosition, playerPosition) <= 0.5f)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
