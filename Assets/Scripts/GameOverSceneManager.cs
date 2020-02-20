﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverSceneManager : MonoBehaviour
{

    [SerializeField]
    private GameObject camObject;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Image typeImage;
    [SerializeField]
    private List<Sprite> typeImages;

    [SerializeField]
    private AudioClip gameoverSound;
    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        scoreText.text = GameStats.latestScore.ToString();
        typeImage.sprite = typeImages[GameStats.gameoverType];

        if (gameoverSound)
        {
            audioSrc.PlayOneShot(gameoverSound);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(loadGameWait(1f));
    }

    IEnumerator loadGameWait(float time)
    {
        Debug.Log("restart");
        camObject.GetComponent<OVRScreenFade>().fadeTime = time;
        camObject.GetComponent<OVRScreenFade>().FadeOut();
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
