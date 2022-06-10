using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ScreensManager : MonoBehaviour
{
    #region EXPOSED_FIELD
    [Header("Screens")]
    [SerializeField] private GameObject viewPause;
    [SerializeField] private GameObject viewEnd;

    [Header("Scene name")]
    [SerializeField] private string sceneMenu = "Menu";
    #endregion

    #region PRIVATE_FIELD
    private bool pauseState = true;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        DisablePause();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            DisablePause();
        }
    }
    #endregion

    #region PUBLIC_FUNCTIONS
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

    public void GoToScene()
    {
        EditorSceneManager.LoadScene(sceneMenu);
    }
    #endregion
}
