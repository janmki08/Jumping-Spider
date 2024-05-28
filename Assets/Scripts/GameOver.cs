using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI; // 게임 오버 UI 오브젝트
    private bool isGameOver = false; // 게임 오버 상태 플래그

    private void Start()
    {
        // 게임 오버 UI와 재시작 버튼 비활성화
        gameOverUI.SetActive(false);
    }

    // 플레이어가 적에게 부딪혔을 때 호출되는 메서드
    public void OnPlayerCollideWithEnemy()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
    }
}