using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BerserkPromp : MonoBehaviour
{

    private void OnEnable()
    {
        Movement.OnBerserkModeStart += activatePromp;
        Movement.OnBerserkModeEnd += activatePromp;
    }

    private void OnDisable()
    {
        Movement.OnBerserkModeStart -= activatePromp;
        Movement.OnBerserkModeEnd -= activatePromp;
    }

    private void activatePromp()
    {
        gameObject.SetActive(gameObject.activeSelf);
    }
}
