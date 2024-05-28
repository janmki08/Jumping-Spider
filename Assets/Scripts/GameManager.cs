using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject gameManagerObject = new GameObject("GameManager");
                    _instance = gameManagerObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public int maxBullets = 3; // 최대 총알 개수
    public float rechargeTime = 12f; // 총알 복구 시간 (초)
    public float cooldownTime = 3f; // 쿨다운 시간 (초)
    private int currentBullets; // 현재 총알 개수
    public Image[] bulletImages; // 총알 스프라이트 UI 배열
    public AudioClip rechargeSound; // 재장전 완료 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private void Start()
    {
        currentBullets = maxBullets; // 초기 총알 개수 설정
        InvokeRepeating("RechargeBullet", rechargeTime, rechargeTime); // 총알 복구 반복 호출
        audioSource = GetComponent<AudioSource>();
    }

    public bool HasBullets()
    {
        return currentBullets > 0;
    }

    public void DecreaseBullet()
    {
        if (currentBullets > 0)
        {
            currentBullets--;
            UpdateBulletCountUI();
        }
    }

    private void RechargeBullet()
    {
        if (currentBullets < maxBullets)
        {
            currentBullets++;
            UpdateBulletCountUI();

            audioSource.PlayOneShot(rechargeSound);
        }
    }

    private void UpdateBulletCountUI()
    {
        for (int i = 0; i < bulletImages.Length; i++)
        {
            if (i < currentBullets)
            {
                bulletImages[i].color = Color.white;
            }
            else
            {
                bulletImages[i].color = new Color(1f, 1f, 1f, 0.15f);
            }
        }
    }
}