using System;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenBehaviour : MonoBehaviour
{
    #region EXPOSED_METHODS
    [SerializeField] private Button btnRestart = null;
    [SerializeField] private Button btnExit = null;
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action onRestart, Action onExit)
    {
        btnRestart.onClick.AddListener(() => { onRestart(); });
        btnExit.onClick.AddListener(() => { onExit(); });
    }
    #endregion
}
