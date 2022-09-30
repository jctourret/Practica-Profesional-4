using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public static Action<SO_Level> OnStart;
    public static Action<SO_Level> OnLevelSelection;
    public static Action<SO_Level> OnLevelchange;

    [Header("Transitioner")]
    [SerializeField] private SceneTransition sceneTransition;

    [Header("Currents")]
    [SerializeField] private int currentStars;
    [SerializeField] private int currentLevel;

    [Header("Levels")]
    [SerializeField] private List<SO_Level> listOfLevels;

    private void Start()
    {
        OnStart?.Invoke(listOfLevels[currentLevel]);
    }
    public void LeftButton()
    {
        if(currentStars >= listOfLevels[(currentLevel--) % listOfLevels.Count].starsRequired)
        {
            currentLevel = (currentLevel--) % listOfLevels.Count;
            OnLevelchange?.Invoke(listOfLevels[currentLevel]);
        }
    }
    public void RightButton()
    {
        if (currentStars >= listOfLevels[(currentLevel++) % listOfLevels.Count].starsRequired)
        {
            currentLevel = (currentLevel++) % listOfLevels.Count;
            OnLevelchange?.Invoke(listOfLevels[currentLevel]);
        }
    }

    public void PlayButton()
    {
        sceneTransition.ChangeAnimation(1, () => {
            ScenesLoaderHandler.LoadScene(listOfLevels[currentLevel].levelName);
        });
    }

    public void MenuButton()
    {
        sceneTransition.ChangeAnimation(1, () => {
            ScenesLoaderHandler.LoadScene("MainMenu");
        });
    }
}
