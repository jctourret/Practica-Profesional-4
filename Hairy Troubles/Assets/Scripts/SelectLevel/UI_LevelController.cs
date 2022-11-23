using UnityEngine;

public class UI_LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private SO_Level level;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    #endregion

    #region UNITY_CALLS
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
    #endregion

    #region PRIVATE_CALLS
    private void ChangePanel(SO_Level currentLevel)
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

    private void ChangeMarker(SO_Level currentLevel)
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
    #endregion
}