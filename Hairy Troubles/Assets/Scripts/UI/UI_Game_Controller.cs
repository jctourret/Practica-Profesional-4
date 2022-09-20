using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UI_Game_Controller : MonoBehaviour
{
    #region EXPOSED_FIELD
    [Header("Stars")]
    [Header("--- STARS ---")]
    [SerializeField] private GameObject[] enableStars;

    [Header("--- PROGRESS ---")]

    [SerializeField] private GameObject progressBar;
    [Header("Percentage")]
    [SerializeField] private Image percentageBar;
    [SerializeField] private TextMeshProUGUI percentageText;

    [Header("--- COMBO ---")]
    [SerializeField] private Slider comboBar;
    [SerializeField] private float depleteRate;
    private bool growthLock;
    private bool declineLock;

    [Space(15)]
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Screens")]
    [SerializeField] private GameObject viewEnd;
    [SerializeField] private GameObject viewPause;
    [SerializeField] private GameObject timerObj;

    [SerializeField] private string sceneMenu = "MainMenu";

    [Header("Transitioner")]
    [SerializeField] private SceneTransition transitioner;
    #endregion

    #region PRIVATE_FIELD
    private float currentPercentage = 0f;
    private float maxPercentage = 100f;

    private bool pauseState = true;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        for (int i = 0; i < enableStars.Length; i++)
        {
            enableStars[i].SetActive(false);
        }
        DisablePause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisablePause();
        }
        if(comboBar.value > 0.0f && !declineLock)
        {
            comboBar.value -= Time.deltaTime * depleteRate;
        }
        else if (growthLock)
        {
            SetGrowthLock(false);
        }
    }
    #endregion

    #region PUBLIC_CALLS
    
    public void ChargeComboBar(int targetDestructibles)
    {
        if(!growthLock)
        {
            comboBar.value += comboBar.maxValue / targetDestructibles;
        }
        if(comboBar.value >= comboBar.maxValue)
        {
            SetGrowthLock(true);
            SetDeclineLock(true);
        }
    }
    public bool CheckComboBar()
    {
        if(comboBar.value >= comboBar.maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetGrowthLock(bool value)
    {
        growthLock = value;
    }

    public void SetDeclineLock(bool value)
    {
        declineLock = value;
    }
    public void ActivateStar(int i)
    {
        enableStars[i].SetActive(true);
    }

    public void UpdateTimer(float time)
    {
        timerText.text = time.ToString("0");
    }

    public void ActivateMenu(bool state)
    {
        viewEnd.SetActive(state);
        progressBar.SetActive(!state);
        timerObj.SetActive(!state);
    }

    public void OnRestartScene(bool isPause)
    {
        if(isPause) DisablePause();

        transitioner.ChangeAnimation(1, ()=> 
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
    
    public void OnGoToScene(bool isPause)
    {
        if(isPause) DisablePause();

        transitioner.ChangeAnimation(1, () =>
        {
            SceneManager.LoadScene(sceneMenu);
        });
    }

    public void SetMaximumProgress(float value)
    {
        maxPercentage = value;
    }

    public void UpdateProgressBar(float current)
    {
        currentPercentage += current;
        percentageBar.fillAmount = currentPercentage / maxPercentage;

        percentageText.text = "%" + (percentageBar.fillAmount * 100).ToString("0");
    }

    public void DisablePause()
    {
        pauseState = !pauseState;

        if (!pauseState)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        viewPause.SetActive(pauseState);
    }
    #endregion
}