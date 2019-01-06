﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class OpeningMovie : MonoBehaviour
{
    public MovieTexture movTexture;//影片
    private AudioSource AS_mov;//影片音軌

    void Start()
    {
        GetComponent<RawImage>().texture = movTexture;
        AS_mov = GetComponent<AudioSource>();
        AS_mov.clip = movTexture.audioClip;//這個MovieTexture的音軌
        movTexture.Play();
        AS_mov.Play();
    }

    void Update()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(15f);//括號內填入影片時間
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");//載入場景
    }
}
