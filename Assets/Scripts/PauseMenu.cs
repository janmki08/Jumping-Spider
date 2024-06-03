using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string SceneToLoad;

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            Time.timeScale = 1f; // 정상 속도로 변경
            SceneManager.LoadScene(SceneToLoad);
        }
        else if (Input.GetKeyDown("q"))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}