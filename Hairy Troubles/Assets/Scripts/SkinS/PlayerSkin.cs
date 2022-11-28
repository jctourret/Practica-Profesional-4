using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [Header("Skin Slots")]
    [SerializeField] List<SkinnedMeshRenderer> skinSlots;
    public static Func<List<SO_Skin>> OnRecieveSkin;

    private void Start()
    {
        ApplySkins(OnRecieveSkin?.Invoke());
    }

    void ApplySkins(List<SO_Skin> skins)
    {
        if(skins != null)
        {
            for (int i = 0; i < skins.Count; i++)
            {
                skinSlots[i].sharedMesh = skins[i].mesh;
                skinSlots[i].material = skins[i].material;
            }
        }
    }
}
