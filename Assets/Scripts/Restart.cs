using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public GameObject restartUI; // 게임 오버 UI 오브젝트
    private void Start()
    {
        // 재시작 버튼 비활성화
        restartUI.SetActive(false);
    }
    private void Update()
    {
        // R 버튼을 눌렀을 때 게임 재시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
    // 플레이어가 적에게 부딪혔을 때 호출되는 메서드
    public void OnPlayerCollideWithEnemy()
    {
        restartUI.SetActive(true);
        // 게임 오버 처리 로직 추가 (예: 플레이어 비활성화, 게임 일시정지 등)
    }
    public void RestartGame()
    {
        // 현재 씬을 다시 로드하여 게임 재시작
        SceneManager.LoadScene("Main");
    }
}
