using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Restart : MonoBehaviour
{
    public GameObject restartUI; // 게임 오버 UI 오브젝트
    public AudioClip audioClip;
    public AudioSource audioSource;
    private Image fadeImage; // 페이드 이미지 컴포넌트

    private void Start()
    {
        // 재시작 버튼 비활성화
        restartUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        // FadeImage 오브젝트 찾기
        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
    }

    private void Update()
    {
        // R 버튼을 눌렀을 때 게임 재시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(FadeAndRestart());
        }
    }

    // 플레이어가 적에게 부딪혔을 때 호출되는 메서드
    public void OnPlayerCollideWithEnemy()
    {
        restartUI.SetActive(true);
        // 게임 오버 처리 로직 추가 (예: 플레이어 비활성화, 게임 일시정지 등)
    }

    private IEnumerator FadeAndRestart()
    {
        // 1초 동안 화면을 점점 어둡게 만듦
        for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime)
        {
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // 씬 재시작
        SceneManager.LoadScene("Main");
    }
}