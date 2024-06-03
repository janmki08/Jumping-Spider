using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TitleTextSizeAnimation : MonoBehaviour
{
    public List<TMP_Text> titleTexts = new List<TMP_Text>(); // TMP_Text 컴포넌트 리스트
    public float scaleSpeed = 2f; // 텍스트 크기 변화 속도
    public float scaleRange = 0.5f; // 텍스트 크기 변화 범위
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
        scaleTimer = Random.Range(0f, scaleSpeed);
    }

    private void Update()
    {

        for (int i = 0; i < titleTexts.Count; i++)
        {
            TMP_Text text = titleTexts[i];

            // 텍스트 크기 변화 효과
            scaleTimer += Time.deltaTime;
            float scale = 1f + Mathf.Sin(scaleTimer * scaleSpeed) * scaleRange;
            text.rectTransform.localScale = new Vector3(scale, scale, scale);

        }
    }
}