using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private SO_Level level;
    [SerializeField] private GameObject panel;
    [SerializeField] private Image house;
    [SerializeField] private Image brokenHouse;
    [SerializeField] private List<Image> Stars;
    [SerializeField] private TextMeshProUGUI progress;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    #endregion

    #region UNITY_CALLS
    private void OnEnable()
    {
        LevelSelectionManager.OnStart += UpdateUI;
        LevelSelectionManager.OnStart += ChangePanel;
        LevelSelectionManager.OnStart += ChangeMarker;
        LevelSelectionManager.OnLevelchange += ChangePanel;
        LevelSelectionManager.OnLevelchange += ChangeMarker;
    }

    private void OnDisable()
    {
        LevelSelectionManager.OnStart -= UpdateUI;
        LevelSelectionManager.OnStart -= ChangePanel;
        LevelSelectionManager.OnStart -= ChangeMarker;
        LevelSelectionManager.OnLevelchange -= ChangePanel;
        LevelSelectionManager.OnLevelchange -= ChangeMarker;
    }
    #endregion

    #region PRIVATE_CALLS
    private void ChangePanel(SO_Level currentLevel, HairyTroublesData data)
    {
        if (level == currentLevel)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    private void ChangeMarker(SO_Level currentLevel, HairyTroublesData data)
    {
        if (level == currentLevel)
        {
            right.SetActive(true);
            left.SetActive(true);
        }
        else
        {
            right.SetActive(false);
            left.SetActive(false);
        }
    }

    private void UpdateUI(SO_Level currentLevel, HairyTroublesData data)
    {
        if (data._levelClear[currentLevel.levelNumber])
        {
            brokenHouse.enabled = true;
            house.enabled = false;
        }
        else
        {
            brokenHouse.enabled = false;
            house.enabled = true;
        }
        progress.text = data._levelProgress.ToString()+"%";
        for(int i = 0; i == data._levelStars[currentLevel.levelNumber]; i++)
        {
            Stars[i].enabled = true;
        }
    }
    #endregion
}