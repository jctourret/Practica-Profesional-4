using UnityEngine;

public class UI_StarPoints_Controller : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField] private GameObject[] enableStars;

    // ----------------

    private void Awake()
    {
        for (int i = 0; i < enableStars.Length; i++)
        {
            enableStars[i].SetActive(false);
        }
    }

    // ----------------

    public void ActivateStar(int i)
    {
        enableStars[i].SetActive(true);
    }
}