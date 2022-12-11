using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BerserkPromp : MonoBehaviour
{

    private void OnEnable()
    {
        GameManager.OnComboBarFull +=activatePromp;
        Movement.OnBerserkModeStart += activatePromp;
    }

    private void OnDisable()
    {
        GameManager.OnComboBarFull -= activatePromp;
        Movement.OnBerserkModeStart -= activatePromp;
    }

    private void activatePromp()
    {
        gameObject.SetActive(gameObject.activeSelf);
    }
}
