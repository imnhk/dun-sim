﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private float gameLength = 180;

    private ElevatorAnimation elevator;
    private GameObject player;

    private Timer timer;
    public enum GAMEOVER { PASSED_OUT = 1, KICKED_OUT = 2, TIMEOUT = 3 }


    public float LeftTime { get { return timer.LeftTime; } }
    public bool IsOn { get { return timer.IsActive; } }

    private int score;
    private float alcohol;

    public void AddScore(int value)
    {
        if (!IsOn)
            return;

        UIManager.Instance.MakeDamagePopup(value);
        score += value;

    }
    public int Score
    {
        get { return score; }
    }
    public float Alcohol
    {
        get { return alcohol; }
        set
        {
            alcohol = value;
            if (alcohol < 0) alcohol = 0;
        }
    }

    private void Awake()
    {
        // Singleton pattern
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        elevator = GameObject.Find("Elevator").GetComponent<ElevatorAnimation>();
        player = GameObject.Find("OVRPlayerController");
    }

    void Start()
    {
        timer = new Timer(gameLength);
        score = 0;
        alcohol = 0;
        StartCoroutine(StartGameIn(1f));
    }

    IEnumerator StartGameIn(float time)
    {
        yield return new WaitForSeconds(1f);
        timer.Start();
        StartCoroutine(elevator.OpenDoorFor(3f));
        
    }

    void Update()
    {
        timer.Update();

        if (!timer.IsActive)
            return;

        if (timer.IsOver)
        {
            GameOver(GAMEOVER.TIMEOUT);
            return;
        }

        if(alcohol >= 0.3f)
        {
            GameOver(GAMEOVER.PASSED_OUT);
            // 만취엔딩
            return;
        }
    }

    public void GameOver(GAMEOVER type)
    {
        timer.Stop();
        Debug.Log("Game Over! " + type);
        GameStats.latestScore = score;
        GameStats.latestTime = timer.LeftTime;

        switch (type)
        {
            case GAMEOVER.PASSED_OUT:
                StartCoroutine(playerPassout());

                break;
            case GAMEOVER.KICKED_OUT:

                break;
            case GAMEOVER.TIMEOUT:

                break;
        }
    }

    public void DebugGameover(int type)
    {
        GameOver((GAMEOVER)type);
    }
    public IEnumerator playerPassout()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<OVRPlayerController>().enabled = false;

        UIManager.Instance.cam.gameObject.GetComponent<OVRScreenFade>().fadeTime = 1f;
        UIManager.Instance.cam.gameObject.GetComponent<OVRScreenFade>().FadeOut();

        float elapsedTime = 0;

        while(elapsedTime < 1f)
        {
            player.transform.Rotate(Vector3.right, 90 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 게임오버 Scene으로
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
}


public class Timer
{
    private float leftTime;
    private bool isActive;
    private float startTime;

    public float LeftTime { get { return leftTime; } }
    public bool IsOver{ get { return leftTime <= 0; } }
    public bool IsActive { get { return isActive; } }

    public Timer(float startTime)
    {
        this.startTime = startTime;
        Reset();
    }


    public void Update()
    {
        if (isActive == false)
            return;

        leftTime -= Time.deltaTime;

        if (IsOver)
        {
            Stop();
            leftTime = 0;
        }
    }

    public void Reset()
    {
        leftTime = startTime;
        Stop();
    }

    public void Start()
    {
        if (IsOver)
            return;
        isActive = true;
    }

    public void Stop()
    {
        isActive = false;
    }
        
}
