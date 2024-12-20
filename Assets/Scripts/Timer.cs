using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeDuration = 3f * 60;

    [SerializeField]
    private bool countDown = true;

    private float timer;
    [SerializeField]
    private Text firstMinute;
    [SerializeField]
    private Text secondMinute;
    [SerializeField]
    private Text separator;
    [SerializeField]
    private Text firstSecond;
    [SerializeField]
    private Text secondSecond;

    private float flashTimer;
    private float flashDuration = 1f;

    public bool start;
    // Start is called before the first frame update
    void Start()
    {
        start = false;
        ResetTimer();


    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (countDown && timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerDisplay(timer);
            }
            else if (!countDown && timer < timeDuration)
            {
                timer += Time.deltaTime;
                UpdateTimerDisplay(timer);
            }
            else
            {
                Flash();
            }
        }



    }

    public void ResetTimer()
    {
        if (countDown)
        {
            timer = timeDuration;
        }
        else
        {
            timer = 0;
        }
        

    }

    private void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        secondMinute.text = currentTime[0].ToString();
        firstMinute.text = currentTime[1].ToString();
        secondSecond.text = currentTime[2].ToString();
        firstSecond.text = currentTime[3].ToString();

    }

    private void Flash()
    {
        if (countDown && timer != 0)
        {
            timer = 0;
            UpdateTimerDisplay(timer);
        }

        if (!countDown && timer != timeDuration)
        {
            timer = 0;
            UpdateTimerDisplay(timer);
        }

        if (flashTimer <= 0)
        {
            flashTimer = flashDuration;

        }
        else if (flashTimer >= flashDuration / 2)
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(false);

        }
        else
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(true);
        }
    }
    private void SetTextDisplay(bool enable)
    {
        firstMinute.enabled = enable;
        secondMinute.enabled = enable;
        separator.enabled = enable;
        firstSecond.enabled = enable;
        secondSecond.enabled = enable;
    }
}
