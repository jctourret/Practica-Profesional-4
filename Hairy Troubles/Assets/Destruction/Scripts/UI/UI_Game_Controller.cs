using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UI_Game_Controller : MonoBehaviour
{
    [Header("Stars")]
    [Header("--- STARS ---")]
    [SerializeField] private GameObject[] enableStars;

    [Header("Percentage")]
    [Header("--- PERCENTAGE ---")]
    [SerializeField] private Image percentageBar;
    [SerializeField] private TextMeshProUGUI percentageText;
    private float currentPercentage = 0f;
    private float maxPercentage = 100f;

    [Space(15)]
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Menu Layer")]
    [SerializeField] private GameObject menuLayer;
    [SerializeField] private GameObject timerObj;

    // ----------------

    private void Awake()
    {
        for (int i = 0; i < enableStars.Length; i++)
        {
            enableStars[i].SetActive(false);
        }
    }

    // ----------------

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
        menuLayer.SetActive(state);
        timerObj.SetActive(!state);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ------------------------

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
}