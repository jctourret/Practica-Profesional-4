using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class UI_Game_Controller : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField] private GameObject[] enableStars;

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
}