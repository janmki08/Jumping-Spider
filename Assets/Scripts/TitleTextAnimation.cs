using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TitleTextAnimation : MonoBehaviour
{
    public List<TMP_Text> titleTexts = new List<TMP_Text>(); // TMP_Text 컴포넌트 리스트
    public float moveSpeed = 1f; // 움직임 속도
    public float moveAmplitude = 0.5f; // 움직임 진폭
    public float bounceInterval = 1f; // 튀어나오는 간격

    public bool applyScaleAnimation = false; // 크기 변화 효과 적용 여부
    public float scaleSpeed = 2f; // 텍스트 크기 변화 속도
    public float scaleRange = 0.5f; // 텍스트 크기 변화 범위

    private float currentTime;
    private float bounceTimer;
    private float scaleTimer;
    private List<Vector2> initialPositions = new List<Vector2>(); // 초기 위치 리스트

    private void Start()
    {
        foreach (TMP_Text text in titleTexts)
        {
            if (text == null)
            {
                Debug.LogError("TMP_Text 컴포넌트가 할당되지 않았습니다. TMP_Text 컴포넌트를 할당해주세요.");
                continue;
            }
            initialPositions.Add(text.rectTransform.anchoredPosition);
        }
        bounceTimer = Random.Range(0f, bounceInterval);
        scaleTimer = Random.Range(0f, scaleSpeed);
    }

    private void Update()
    {
        currentTime += Time.deltaTime * moveSpeed;
        bounceTimer += Time.deltaTime;

        for (int i = 0; i < titleTexts.Count; i++)
        {
            TMP_Text text = titleTexts[i];

            // 텍스트 움직임 효과
            if (bounceTimer >= bounceInterval)
            {
                bounceTimer = 0f;
                text.rectTransform.anchoredPosition = initialPositions[i] + new Vector2(Random.Range(-moveAmplitude, moveAmplitude), Random.Range(-moveAmplitude, moveAmplitude));
            }
            else
            {
                float x = Mathf.Sin(currentTime) * moveAmplitude;
                float y = Mathf.Cos(currentTime) * moveAmplitude;
                text.rectTransform.anchoredPosition = initialPositions[i] + new Vector2(x, y);
            }
        }
    }
}