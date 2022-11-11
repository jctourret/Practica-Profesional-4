using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkinSlot : MonoBehaviour
{
    enum skinStates
    {
        Locked,
        Unlocked,
        Equipped,
    }
    static public Func<int> OnGetCurrentStars;
    static public Action<SO_Skin> OnSkinEquipped;
    [Header("Slotted Skin")]
    [SerializeField] private SO_Skin skin;
    [SerializeField] private TextMeshProUGUI tmp;
    [Header("Buttons")]
    List<GameObject> buttons;
    skinStates currentState = skinStates.Locked;

    private void Awake()
    {
        tmp.text = skin.starsRequired.ToString();
    }

    private void OnEnable()
    {
        SkinsManager.OnSkinChange += checkUnequipped;
    }
    private void OnDisable()
    {
        SkinsManager.OnSkinChange += checkUnequipped;
    }

    private void Start()
    {
        if (OnGetCurrentStars?.Invoke() >= skin.starsRequired)
        {
            currentState = skinStates.Unlocked;
        }
        else
        {
            currentState = skinStates.Locked;
        }
        for (int i = 0; i >= buttons.Count; i++)
        {
            if (i != ((int)currentState))
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

    }

    void checkUnequipped(SO_Skin checkedSkin)
    {
        if (checkedSkin.skinSlot == skin.skinSlot)
        {
            if (checkedSkin != skin)
            {
                currentState = skinStates.Unlocked;
            }
        }
    }

    public void EquipSkin()
    {
        int currentStars = (int)OnGetCurrentStars?.Invoke();
        if (currentState == skinStates.Unlocked && currentStars >= skin.starsRequired)
        {
            currentState = skinStates.Equipped;
            OnSkinEquipped?.Invoke(skin);
        }
        else
        {
            //Error sound
        }
    }
}
