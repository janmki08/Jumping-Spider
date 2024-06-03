using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play(); // BGM 재생
    }

    private void OnDestroy()
    {
        audioSource.Stop(); // BGM 중지
    }
}
