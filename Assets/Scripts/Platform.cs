using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;
    public AudioClip jumpSound; // 점프 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private void Start()
    {
        // 오디오 소스 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.y <= 0.5f)
        {
            Rigidbody2D rb = other.collider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;

                // 점프 사운드 재생
                if (jumpSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }
            }
        }
    }
}
