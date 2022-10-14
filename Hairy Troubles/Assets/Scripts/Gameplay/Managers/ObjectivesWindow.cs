using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesWindow : MonoBehaviour
{
    #region PRIVATE_METHODS
    [SerializeField] private Button btnPlay = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        this.gameObject.SetActive(true);
    }
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action play)
    {
        btnPlay.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            play();
        });
    }
    #endregion
}