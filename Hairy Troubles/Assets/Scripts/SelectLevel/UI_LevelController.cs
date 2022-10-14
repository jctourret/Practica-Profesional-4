using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelController : MonoBehaviour
{
    [SerializeField] private SO_Level level;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    private void OnEnable()
    {
        LevelSelectionManager.OnStart += ChangePanel;
        LevelSelectionManager.OnStart += ChangeMarker;
        LevelSelectionManager.OnLevelchange += ChangePanel;
        LevelSelectionManager.OnLevelchange += ChangeMarker;
    }

    private void OnDisable()
    {
        LevelSelectionManager.OnStart -= ChangePanel;
        LevelSelectionManager.OnStart -= ChangeMarker;
        LevelSelectionManager.OnLevelchange -= ChangePanel;
        LevelSelectionManager.OnLevelchange -= ChangeMarker;
    }

    void ChangePanel(SO_Level currentLevel)
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
    void ChangeMarker(SO_Level currentLevel)
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
}
