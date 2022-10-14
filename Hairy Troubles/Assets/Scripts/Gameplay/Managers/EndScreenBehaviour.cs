using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreenBehaviour : MonoBehaviour
{
    #region EXPOSED_METHODS
    [Header("Stars")]
    [SerializeField] private GameObject[] enableStars = null;

    [Header("Percentage")]
    [SerializeField] private Image finalFrame = null;
    [SerializeField] private TextMeshProUGUI txtFinalPercentage = null;
    [SerializeField] private Sprite[] typesOfFrames = null;

    [Header("Buttons")]
    [SerializeField] private Button btnRestart = null;
    [SerializeField] private Button btnExit = null;
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action onRestart, Action onExit)
    {
        for (int i = 0; i < enableStars.Length; i++)
        {
            enableStars[i].SetActive(false);
        }

        btnRestart.onClick.AddListener(() => { onRestart(); });
        btnExit.onClick.AddListener(() => { onExit(); });
    }

    public void ActivateFinalStars(int i)
    {
        enableStars[i].SetActive(true);
        finalFrame.sprite = typesOfFrames[i];
    }

    public void SetFinalPercentage(string percentage)
    {
        txtFinalPercentage.text = percentage;
    }

    public void SetActive(bool state)
    {
        this.gameObject.SetActive(state);
    }
    #endregion
}
