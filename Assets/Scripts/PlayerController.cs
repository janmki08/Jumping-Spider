using System.Collections;
using UnityEngine;

// 스크립트가 정상적으로 동작하는 데 필요한 컴포넌트들이 항상 존재하도록 보장합니다.
public class PlayerController : MonoBehaviour
{
    // --- 상태 정의 ---
    private enum PlayerState { Normal, Grappling }
    private PlayerState currentState;

    // --- 인스펙터 설정 변수 ---
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float pendulumForce = 5f;

    [Header("Item Effects")]
    [SerializeField] private float itemEffectDuration = 2f;
    [SerializeField] private float itemUpwardForce = 10f;
    [SerializeField] private float invincibilityDuration = 2f;

    [Header("Visuals")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite jumpingSprite;
    [SerializeField] private Sprite fallingSprite;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip swingSound;

    [Header("Dependencies")]
    [SerializeField] private GameOver gameOver;
    [SerializeField] private Restart restartScript;

    // --- 내부 상태 변수 ---
    private float moveInput;
    private bool isItemEffectActive = false;
    private bool isInvincible = false;
    private Coroutine blinkCoroutine;

    // --- 컴포넌트 참조 ---
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GrapplingHook grappling;
    private Collider2D col;
    private AudioSource audioSource;

    // --- 공개 프로퍼티 ---
    public bool IsGrappling => currentState == PlayerState.Grappling;
    public bool IsInvincible => isInvincible;

    private void Awake()
    {
        // 컴포넌트 참조를 한 번만 가져옵니다.
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        grappling = GetComponent<GrapplingHook>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // 초기 상태 설정
        currentState = PlayerState.Normal;
    }

    private void Update()
    {
        // 입력은 Update에서 처리하는 것이 좋습니다.
        moveInput = Input.GetAxis("Horizontal");

        // 상태 전환 로직
        PlayerState nextState = grappling.isAttach ? PlayerState.Grappling : PlayerState.Normal;
        if (nextState != currentState)
        {
            TransitionToState(nextState);
        }

        // 매 프레임마다 스프라이트를 업데이트합니다.
        UpdateSprite();
    }

    void FixedUpdate()
    {
        // 현재 상태에 따라 물리 로직을 실행합니다.
        switch (currentState)
        {
            case PlayerState.Normal:
                HandleNormalMovement();
                break;
            case PlayerState.Grappling:
                HandleGrapplingMovement();
                break;
        }
    }

    private void TransitionToState(PlayerState newState)
    {
        // 이전 상태 정리
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        sprite.enabled = true; // 깜빡임 종료 후 항상 보이도록 설정

        // 새 상태로 전환
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Normal:
                col.enabled = true;
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                // 관성 적용
                if (grappling.isReleased)
                {
                    rb.velocity = grappling.releaseVelocity;
                    grappling.isReleased = false;
                }
                break;

            case PlayerState.Grappling:
                col.enabled = false;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = swingSound;
                    audioSource.Play();
                }
                // 그래플링 중 깜빡임 효과 시작
                blinkCoroutine = StartCoroutine(BlinkEffect(grappling.grappleDuration, blinkInterval));
                break;
        }
    }

    private void HandleNormalMovement()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    private void HandleGrapplingMovement()
    {
        Vector2 hookPosition = grappling.GetHookPosition();
        Vector2 pendulumDirection = (hookPosition - (Vector2)transform.position).normalized;

        float pendulumSpeed = Vector2.Dot(rb.velocity, pendulumDirection);

        // 진자 운동 효과 적용
        rb.AddForce(-pendulumDirection * pendulumSpeed * pendulumForce);

        // 좌우 이동 입력 적용
        Vector2 perpendicularDirection = -Vector2.Perpendicular(pendulumDirection);
        rb.AddForce(perpendicularDirection * moveInput * speed);
    }

    private void UpdateSprite()
    {
        if (rb.velocity.y > 0.1f)
        {
            sprite.sprite = jumpingSprite;
        }
        else if (rb.velocity.y < -0.1f)
        {
            sprite.sprite = fallingSprite;
        }
        else
        {
            sprite.sprite = idleSprite;
        }
    }

    // 리스타트
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                // 무적 상태가 아닐 때만 게임 오버 처리
                if (gameOver != null)
                {
                    gameOver.OnPlayerCollideWithEnemy();
                }
                if (restartScript != null)
                {
                    restartScript.OnPlayerCollideWithEnemy();
                }
            }
        }
    }

    public void ApplyItemEffect()
    {
        if (!isItemEffectActive)
        {
            // 아이템 효과 활성화
            isItemEffectActive = true;
            Invoke(nameof(DeactivateItemEffect), itemEffectDuration);

            // 위쪽 방향으로 힘 적용
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * itemUpwardForce, ForceMode2D.Impulse);

            // 무적 효과 활성화
            StartCoroutine(InvincibilityEffect());
        }
    }

    private void DeactivateItemEffect()
    {
        // 아이템 효과 비활성화
        isItemEffectActive = false;
    }

    private IEnumerator InvincibilityEffect()
    {
        isInvincible = true;
        // 무적 상태일 때의 깜빡임 효과 시작
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkEffect(invincibilityDuration, blinkInterval));

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        // 깜빡임 코루틴이 아직 실행 중이면 중지하고 스프라이트를 다시 활성화합니다.
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        sprite.enabled = true;
    }

    private IEnumerator BlinkEffect(float duration, float interval)
    {
        float timer = 0f;
        while (timer < duration)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
        sprite.enabled = true; // 효과가 끝나면 항상 보이도록 설정
    }
}