using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    #region EXPOSED_METHODS
    [SerializeField] private TMP_Text cdText = null;
    [SerializeField] private Animator animator = null;
    #endregion

    #region PRIVATE_METHODS
    private float cdTimer = 4;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
       this.gameObject.SetActive(false);
    }
    #endregion

    #region PUBLIC_CALLS
    public void StartCountdown()
    {
        this.gameObject.SetActive(true);

        animator.SetTrigger("cdNow");
        StartCoroutine(Countdown());
    }
    #endregion

    #region PRIVATE_CALLS
    private IEnumerator Countdown()
    {
        string cdString;
        
        while(cdTimer > 0)
        {
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
    #endregion
}