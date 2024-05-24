using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public float TotalTimeLeft;
    private float _currentTimeLeft;
    public TMP_Text TimerText;

    public ServingStation ServingStation;
    public Slider TeleportStationCooldown;

    public int OrdersRequired;
    public TMP_Text OrderText;
    public int OrdersServed { get; set; } = 0;

    public SabotageCooldownDoneEvent sabotageCooldownDoneEvent;

    public Transform[] servingStationLocations;

    public AudioSource BackgroundAudio;

    // Start is called before the first frame update
    void Start()
    {
        BackgroundAudio.Play();
        _currentTimeLeft = TotalTimeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTimeLeft > 0)
        {
            _currentTimeLeft -= Time.deltaTime;
        }
        else
        {
            // You Win
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
            Reset();
        }

        if (OrdersServed >= OrdersRequired)
        {
            // You Lose
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
            Reset();
        }
        updateTimer(_currentTimeLeft);
        updateOrders();

        TeleportStationCooldown.value = ServingStation.currentSliderValue;
    }

    void updateTimer(float currentTime)
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    void updateOrders()
    {
        OrderText.text = string.Format("Orders Left: {0}", OrdersRequired - OrdersServed);
    }

    private void Reset()
    {
        _currentTimeLeft = TotalTimeLeft;
        OrdersServed = 0;
    }
}
