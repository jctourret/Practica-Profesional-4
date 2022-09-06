using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    TMP_Text cdText;
    Animator animator;
    float cdTimer = 4;
    void Start()
    {
        cdText = GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        UI_Game_Controller.OnPlayButton += StartCountdown;
    }

    private void OnDisable()
    {
        UI_Game_Controller.OnPlayButton -= StartCountdown;
    }

    public void StartCountdown()
    {
        animator.SetTrigger("cdNow");
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown()
    {
        while(cdTimer > 0)
        {
            string cdString;
            cdTimer -= Time.unscaledDeltaTime;
            if (cdTimer >= 3)
            {
                cdString = "3";
            }
            else if(cdTimer <=0.5)
            {
                cdString = "GO!";
            }
            else
            {
                cdString = cdTimer.ToString("F0");
            }
            cdText.text = cdString;
            yield return null;
        }
        Time.timeScale = 1f;
    }
}
