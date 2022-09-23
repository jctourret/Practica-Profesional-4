using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class SO_Level : ScriptableObject
{
    [SerializeField] private new string name;
    public int sceneIndex;
    [SerializeField] private int starsAchieved;
    public int starsRequired;
    [SerializeField] private Sprite image;
    [SerializeField] private string description;
}
