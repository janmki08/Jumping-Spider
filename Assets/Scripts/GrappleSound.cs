using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleSound : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 웹 스윙 사운드 재생
        if (Input.GetMouseButtonDown(0) && !audioSource.isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
