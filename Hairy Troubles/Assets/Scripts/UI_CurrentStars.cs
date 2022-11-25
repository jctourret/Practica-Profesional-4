using System;
using UnityEngine;
using TMPro;
public class UI_CurrentStars : MonoBehaviour
{
    static public Func<int> OnGetCurrentStars;
    TextMeshProUGUI text;
    private void Start()
    {
        text.text = OnGetCurrentStars?.Invoke().ToString();
    }
}
