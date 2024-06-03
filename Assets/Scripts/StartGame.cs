using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string SceneToLoad;
    private Image fadeImage; // 페이드 이미지 컴포넌트
    public AudioClip audioClip;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // FadeImage 오브젝트 찾기
        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(audioClip);
            Debug.Log("게임 시작!");
            StartCoroutine(FadeAndRestart());
        }
    }

    private IEnumerator FadeAndRestart()
    {
        // 1초 동안 화면을 점점 어둡게 만듦
        for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime)
        {
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene(SceneToLoad);
    }
}