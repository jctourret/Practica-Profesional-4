using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UiGameController : MonoBehaviour
{
    #region EXPOSED_FIELD
    [Header("Stars")]
    [SerializeField] private GameObject[] enableStars = null;

    [Header("Progress")]
    [SerializeField] private GameObject progressBar = null;

    [Header("Percentage")]
    [SerializeField] private Image percentageBar = null;
    [SerializeField] private TextMeshProUGUI percentageText = null;

    [Header("Combo")]
    [SerializeField] private Slider comboBar;
    [SerializeField] private float depleteRate;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText = null;

    [Header("Screens")]
    [SerializeField] private string sceneMenu = "MainMenu";
    [SerializeField] private GameObject viewEnd = null;
    [SerializeField] private GameObject viewPause = null;
    [SerializeField] private GameObject timerObj = null;

    [Header("Class")]
    [SerializeField] private ObjectivesWindow objectivesWindow = null;
    [SerializeField] private CountdownTimer countdownTimer= null;
    [SerializeField] private PauseBehaviour pauseBehaviour = null;
    [SerializeField] private EndScreenBehaviour endScreenBehaviour = null;
    [SerializeField] private PercentageStarsHolder percentageStarsHolder = null;

    [Header("Transitioner")]
    [SerializeField] private SceneTransition transitioner = null;
    #endregion

    #region PRIVATE_FIELD
    private float currentPercentage = 0f;
    private float maxPercentage = 100f;
    private bool pauseState = true;
    private bool growthLock = false;
    private bool declineLock = false;
    #endregion

    #region ACTIONS
    public Action OnPlayButton;
    #endregion

    #region UNITY_CALLS
    private void OnEnable()
    {
        OnPlayButton += countdownTimer.StartCountdown;
    }

    private void OnDisable()
    {
        OnPlayButton -= countdownTimer.StartCountdown;
    }

    private void Start()
    {
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisablePause();
        }

        if (comboBar.value > 0.0f && !declineLock)
        {
            comboBar.value -= Time.deltaTime * depleteRate;
        }
        else if (growthLock && comboBar.value <= 0.0f)
        {
            SetGrowthLock(false);
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void Initialize()
    {
        for (int i = 0; i < enableStars.Length; i++)
        {
            enableStars[i].SetActive(false);
        }
        DisablePause();

        objectivesWindow.Initialize(() => { OnPlayButton?.Invoke(); });
        pauseBehaviour.Initialize(DisablePause, OnRestartScene, OnGoToScene);
        endScreenBehaviour.Initialize(OnRestartScene, OnGoToScene);
    }

    public void SetValues(float finalGoal, float firstPercentGoal, float mediumPercentGoal, float finalPercentGoal)
    {
        SetMaximumProgress(finalGoal);
        percentageStarsHolder.Initialize(firstPercentGoal, mediumPercentGoal, finalPercentGoal);
    }

    public void ActivateFinalStar(int i)
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

    public void ChargeComboBar(int targetDestructibles)
    {
        if (!growthLock)
        {
            comboBar.value += comboBar.maxValue / targetDestructibles;
        }
        if (comboBar.value >= comboBar.maxValue)
        {
            SetGrowthLock(true);
            SetDeclineLock(true);
        }
    }

    public bool CheckComboBar()
    {
        if (comboBar.value >= comboBar.maxValue)
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
    #endregion

    #region PRIVATE_CALLS
    private void DisablePause()
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

    private void OnRestartScene()
    {
        if (pauseState) DisablePause();

        transitioner.ChangeAnimation(1, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void OnGoToScene()
    {
        if (pauseState) DisablePause();

        transitioner.ChangeAnimation(1, () =>
        {
            SceneManager.LoadScene(sceneMenu);
        });
    }
    #endregion
}