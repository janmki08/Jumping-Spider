using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    public AudioClip audioClip;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // PauseUI 비활성화
        pauseUI.SetActive(false);
    }
    private bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        // ESC 키를 눌렀을 때 게임 일시정지/재개
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioSource.PlayOneShot(audioClip);
            isPaused = !isPaused;
            pauseUI.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f; // 0f: 일시정지, 1f: 정상 속도
        }
    }
}
