using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ObjControl : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        UI_Game_Controller.OnPlayButton += playAnimation;
    }

    private void OnDisable()
    {
        UI_Game_Controller.OnPlayButton -= playAnimation;
    }

    void playAnimation()
    {
        animator.SetTrigger("ObjectiveAnim");
    }
}
