using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // 점수를 표시할 텍스트 UI
    public TextMeshProUGUI shadowText; // 점수를 표시할 텍스트 UI

    public TextMeshProUGUI highScoreText; // 최고 기록을 표시할 TextMeshProUGUI
    public TextMeshProUGUI highScoreShadowText; // 최고 기록을 표시할 TextMeshProUGUI
    public Transform camTransform; // 플레이어의 Transform 컴포넌트
    private float initialPlayerY; // 플레이어의 초기 Y 위치
    private float score; // 현재 점수
    private float highScore; // 최고 기록

    private const string HIGH_SCORE_KEY = "HighScore"; // 최고 기록을 저장할 키

    private void Start()
    {
        initialPlayerY = camTransform.position.y; // 플레이어의 초기 Y 위치 저장
        score = 0f; // 점수 초기화
        highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0f); // 저장된 최고 기록 불러오기 (기본값: 0)
        UpdateScoreUI(); // 초기 점수 표시
    }

    private void Update()
    {
        // 플레이어의 현재 Y 위치와 초기 Y 위치의 차이를 계산하여 점수로 사용
        float distance = camTransform.position.y - initialPlayerY;
        score = Mathf.Max(score, distance);
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        // 점수를 텍스트 UI에 표시
        scoreText.text = Mathf.RoundToInt(score).ToString() + " CM";
        shadowText.text = Mathf.RoundToInt(score).ToString() + " CM";
        highScoreText.text = "Best! " + Mathf.RoundToInt(highScore).ToString() + " CM";
        highScoreShadowText.text = "Best! " + Mathf.RoundToInt(highScore).ToString() + " CM";

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, highScore); // 최고 기록 저장
            PlayerPrefs.Save(); // 변경 사항 저장
        }
    }
}
