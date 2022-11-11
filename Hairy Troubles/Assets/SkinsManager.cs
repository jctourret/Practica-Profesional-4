using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    static public Action OnSendCurrentStars;
    static public Action<SO_Skin> OnSkinChange;
    [Header("Stars")]
    [SerializeField] private int currentStars;
    [Header("Skins")]
    [SerializeField] private SO_Skin currentEyesSkin;
    [SerializeField] private SO_Skin currentHatSkin;
    [SerializeField] private SO_Skin currentBodySkin;

    private void OnEnable()
    {
        GameManager.OnSaveStars += SaveStars;
        SkinSlot.OnGetCurrentStars += SendStars;
        SkinSlot.OnSkinEquipped += SkinChange; 
    }
    private void OnDisable()
    {
        GameManager.OnSaveStars -= SaveStars;
        SkinSlot.OnGetCurrentStars -= SendStars;
        SkinSlot.OnSkinEquipped -= SkinChange;
    }
    int SendStars()
    {
        return currentStars;
    }
    void SkinChange(SO_Skin skin)
    {
        switch (skin.skinSlot)
        {
            case SO_Skin.SkinSlot.eyes:
                currentEyesSkin = skin;
                break;
            case SO_Skin.SkinSlot.hat:
                currentHatSkin = skin;
                break;
            case SO_Skin.SkinSlot.body:
                currentBodySkin = skin;
                break;
        }
        OnSkinChange?.Invoke(skin);
    }
    void SaveStars(int savedStars)
    {
        currentStars += savedStars;
    }
}
