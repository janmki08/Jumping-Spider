using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public Image fadeImage; // 페이드 이미지 컴포넌트
    public float fadeInDuration = 1f; // 페이드 인 지속 시간

    private void Start()
    {
        // 초기에 화면을 완전히 어둡게 만듦
        fadeImage.color = Color.black;

        // 페이드 인 코루틴 시작
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 페이드 인 완료 후 페이드 이미지 비활성화
        fadeImage.gameObject.SetActive(false);
    }
}