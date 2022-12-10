using UnityEngine;

public class SaveManager : MonoBehaviour
{
    HairyTroublesData data;
    static SaveManager singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
            data = SaveSystem.LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GameManager.OnSaveStars += SaveProgress;
    }

    private void OnDisable()
    {
        GameManager.OnSaveStars -= SaveProgress;
    }

    void SaveProgress(int stars)
    {
        data._stars += stars;
        SaveSystem.SaveGame(data);
    }
}
