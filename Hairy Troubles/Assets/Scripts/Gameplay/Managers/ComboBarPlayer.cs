using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBarPlayer : MonoBehaviour
{
    #region EXPOSED_FIELD
    [SerializeField] private Slider comboBar;
    [SerializeField] private GameObject peludoNormal;
    [SerializeField] private GameObject peludoBerserk;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float depleteRate;
    #endregion

    #region PRIVATE_FIELD
    private bool growthLock = false;
    private bool declineLock = false;
    #endregion

    #region PUBLIC_CALLS
    public void UpdateGrownState()
    {
        if (comboBar.value > 0.0f && !declineLock)
        {
            comboBar.value -= Time.deltaTime * depleteRate;
        }
        else if (growthLock && comboBar.value <= 0.0f)
        {
            SetGrowthLock(false);
        }
    }

    public void ChargeComboBar(int targetDestructibles)
    {
        if (!growthLock)
        {
            comboBar.value += comboBar.maxValue / targetDestructibles;
        }
        if (comboBar.value >= comboBar.maxValue)
        {
            SetGrowthLock(true);
            SetDeclineLock(true);
        }
    }
    public void changeBerserkUI()
    {
        peludoNormal.SetActive(peludoNormal.activeSelf);
        smoke.gameObject.SetActive(peludoNormal.activeSelf);
        if (!smoke.isPlaying)
        {
            smoke.Play();
        }
        peludoBerserk.SetActive(peludoNormal.activeSelf);
    }
    public bool CheckComboBar()
    {
        if (comboBar.value >= comboBar.maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetDeclineLock(bool value)
    {
        declineLock = value;
    }
    #endregion

    #region PRIVATE_CALLS
    private void SetGrowthLock(bool value)
    {
        growthLock = value;
    }
    #endregion
}
