using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour
{
    private static Gun _instance;
    public static Gun Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Gun>();
                if (_instance == null)
                {
                    GameObject gunObject = new GameObject("Gun");
                    _instance = gunObject.AddComponent<Gun>();
                }
            }
            return _instance;
        }
    }

    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    public float bulletSpeed = 10f; // 총알 속도
    public int maxBullets = 3; // 최대 총알 개수
    public float cooldownTime = 3f; // 쿨다운 시간 (초)
    public float rechargeTime = 12f; // 총알 복구 시간 (초)
    private int currentBullets; // 현재 총알 개수
    private bool canShoot = true; // 발사 가능 여부
    private float lastShotTime; // 마지막 발사 시간

    public AudioClip shootingSound; // 발사 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    public Image gauge;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        // 필요하다면 게이지 초기 설정
        gauge.fillAmount = 0f;
        currentBullets = maxBullets; // 초기 총알 개수 설정
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RechargeRoutine()); // 총알 복구 코루틴 시작
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && canShoot && GameManager.Instance.HasBullets())
        {
            Shoot();
            GameManager.Instance.DecreaseBullet();
            canShoot = false;
            lastShotTime = Time.time; // 마지막 발사 시간 저장
            StartCoroutine(GaugeCooldown(3f));
            Invoke("ResetCooldown", cooldownTime);
        }
    }

    private void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 shootDirection = (mousePosition - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        audioSource.PlayOneShot(shootingSound);
    }

    private IEnumerator RechargeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(rechargeTime);
            if (currentBullets < maxBullets)
            {
                currentBullets++; // 총알 개수 증가
            }
        }
    }
    private void ResetCooldown()
    {
        canShoot = true; // 쿨다운 시간 이후에 발사 가능 상태로 변경
    }
    IEnumerator GaugeCooldown(float cool)
    {
        float initialCool = cool;  // 초기 쿨다운 시간을 저장합니다.
        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;  // 매 프레임마다 쿨다운 시간을 감소시킵니다.
            gauge.fillAmount = cool / initialCool;  // 남은 시간에 비례하여 게이지를 설정합니다.
            yield return new WaitForFixedUpdate();  // 고정된 업데이트를 기다립니다.
        }
        gauge.fillAmount = 0.0f;  // 쿨다운이 완료되면 게이지를 0으로 설정합니다.
    }
}