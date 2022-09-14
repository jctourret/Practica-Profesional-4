using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesWindow : MonoBehaviour
{
    #region PRIVATE_METHODS
    [SerializeField] private Button btnPlay = null;
    [SerializeField] private Animator animator = null;
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action play)
    {
        btnPlay.onClick.AddListener(() =>
        { 
            play();
        });
    }
    public void playAnimation()
    {
        animator.SetTrigger("ObjectiveAnim");
    }
    #endregion
}