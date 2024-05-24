using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public float TimeLeft;
    public bool TimerOn = false;
    public TMP_Text TimerText;

    public int OrdersRequired;
    public TMP_Text OrderText;
    public int OrdersServed { get; set; } = 0;
    // Start is called before the first frame update
    void Start()
    {
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
            }
            else
            {
                // You Win
                //TimeLeft = 0;
                //TimerOn = false;
                //Time.timeScale = 0;
            }

            if (OrdersServed == OrdersRequired)
            {
                // You Lose
                //TimerOn = false;
                //Time.timeScale = 0;
            }
            updateTimer(TimeLeft);
            updateOrders();
        }
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
}
