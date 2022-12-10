using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public HairyTroublesData data;
    public static SaveManager singleton;
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
    public void SaveProgress(int starsEarned, int progressMade, int levelIndex)
    {
        if(starsEarned != 0)
        {
            data._levelClear[levelIndex] = true;
        }
        if (data._levelStars[levelIndex] < starsEarned)
        {
            data._levelStars[levelIndex] = starsEarned;
        }
        if (data._levelProgress[levelIndex] < progressMade)
        {
            data._levelProgress[levelIndex] = progressMade;
        }
        data._stars += starsEarned;
        SaveSystem.SaveGame(data);
    }
}
