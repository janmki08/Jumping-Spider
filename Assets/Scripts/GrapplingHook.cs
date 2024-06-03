using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrapplingHook : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;
    public Vector2 releaseVelocity;
    Vector2 mousedir;

    public bool isHookActive;
    public bool isLineMax;

    public bool isAttach;
    public bool isReleased = false; // 그래플링 해제 시점
    public float grappleDuration = 3f; // 그래플링 지속 시간
    public float grappleCooldown = 1f; // 그래플링 쿨다운 시간
    private float grappleTimer; // 그래플링 타이머
    private float cooldownTimer; // 쿨다운 타이머
    public Image gauge;

    // Start is called before the first frame update
    void Start()
    {
        // 게이지 초기 설정
        gauge.fillAmount = 0f;
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (Input.GetMouseButtonDown(0) && !isHookActive && cooldownTimer <= 0f)
        {
            hook.position = transform.position;
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            isHookActive = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        if (isHookActive && !isLineMax && !isAttach)
        {
            hook.Translate(mousedir.normalized * Time.deltaTime * 15);

            if (Vector2.Distance(transform.position, hook.position) > 5)
            {
                isLineMax = true;
            }
        }
        else if (isHookActive && isLineMax && !isAttach)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 15);
            if (Vector2.Distance(transform.position, hook.position) < 0.1f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }

        if (isAttach)
        {
            grappleTimer += Time.deltaTime;

            // 그래플링이 매달려 있는 동안 쿨다운 게이지 감소
            gauge.fillAmount = 1 - (grappleTimer / grappleDuration);

            if (grappleTimer >= grappleDuration || Input.GetMouseButtonUp(0))
            {
                isAttach = false;
                isHookActive = false;
                isLineMax = false;
                hook.GetComponent<Hookg>().joint2D.enabled = false;
                hook.gameObject.SetActive(false);
                isReleased = true;
                releaseVelocity = GetComponent<Rigidbody2D>().velocity;
                grappleTimer = 0f;
                cooldownTimer = grappleCooldown;

                // 그래플링이 풀리면 쿨다운 게이지 초기화
                gauge.fillAmount = 0f;
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public Vector2 GetHookPosition()
    {
        return hook.position;
    }
}
