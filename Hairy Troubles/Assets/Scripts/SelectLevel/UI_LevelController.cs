using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LevelController : MonoBehaviour
{
    const int levelArrayOffset = 1;
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
        LevelSelectionManager.OnLevelchange += UpdateUI;
        LevelSelectionManager.OnLevelchange += ChangePanel;
        LevelSelectionManager.OnLevelchange += ChangeMarker;
    }

    private void OnDisable()
    {
        LevelSelectionManager.OnStart -= UpdateUI;
        LevelSelectionManager.OnStart -= ChangePanel;
        LevelSelectionManager.OnStart -= ChangeMarker;
        LevelSelectionManager.OnLevelchange -= UpdateUI;
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
            if(data._levelClear[currentLevel.levelNumber - levelArrayOffset])
            {
                right.SetActive(true);
            }
            else
            {
                right.SetActive(false);
            }
            int index = (currentLevel.levelNumber - 1 - levelArrayOffset) % data._levelClear.Length;
            if (index >= 0 && data._levelClear[index])
            {
                left.SetActive(true);
            }
            else
            {
                left.SetActive(false);
            }
        }
        else
        {
            right.SetActive(false);
            left.SetActive(false);
        }
    }

    private void UpdateUI(SO_Level currentLevel, HairyTroublesData data)
    {
        Debug.Log("Update UI");
        int index = level.levelNumber - levelArrayOffset % data._levelClear.Length;
        if (data._levelClear[index])
        {
            brokenHouse.enabled = true;
            house.enabled = false;
        }
        else
        {
            brokenHouse.enabled = false;
            house.enabled = true;
        }
        progress.text = data._levelProgress[index].ToString()+"%";
        for(int i = 0; i < data._levelStars[index]; i++)
        {
            Stars[i].enabled = true;
        }
    }
    #endregion
}