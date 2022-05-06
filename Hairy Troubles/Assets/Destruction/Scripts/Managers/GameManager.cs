using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum MissionsState
    {
        none,
        First,
        Medium,
        Final
    }

    [Header("Scene Points")]
    public float scenePoints = 0;
    public float actualPoints = 0;

    public static float static_scenePoints = 0f;

    [Header("Scene State")]
    [SerializeField] private MissionsState missionsState;
    [Range(0, 400)]
    [SerializeField] private int sceneTime = 60;

    [Header("Determine Percentage Missions")]
    [Range(0, 100)]
    [SerializeField] private int firstPercentGoal = 30;
    [Range(0, 100)]
    [SerializeField] private int mediumPercentGoal = 60;
    [Range(0, 100)]
    [SerializeField] private int finalPercentGoal = 100;

    private float firstGoal = 0f;
    private float mediumGoal = 0f;
    private float finalGoal = 0f;

    private const float PERCENT = 100f;

    private UI_Game_Controller starsController;

    private float timer = 0f;
    private bool playing = true;

    // ----------------

    private void Awake()
    {
        starsController = GetComponent<UI_Game_Controller>();

        missionsState = MissionsState.none;

        timer = sceneTime;

        scenePoints = 0;
        actualPoints = 0;
    }

    private void OnEnable()
    {
        //DestructibleComponent.InScenePoints += GoalPoints;
        DestructibleComponent.DestructionPoints += ChargePoints;
    }

    private void OnDisable()
    {
        static_scenePoints = 0;

        //DestructibleComponent.InScenePoints -= GoalPoints;
        DestructibleComponent.DestructionPoints -= ChargePoints;
    }
    
    private void Start()
    {
        scenePoints = static_scenePoints;

        firstGoal = scenePoints * ((float)firstPercentGoal / PERCENT);
        mediumGoal = scenePoints * ((float)mediumPercentGoal / PERCENT);
        finalGoal = scenePoints * ((float)finalPercentGoal / PERCENT);

        Debug.Log(firstPercentGoal + "% percent: " + firstGoal);
        Debug.Log(mediumPercentGoal + "% percent: " + mediumGoal);
        Debug.Log(finalPercentGoal + "% percent: " + finalGoal);

        // Percentage Bar:
        starsController.SetMaximumProgress(finalGoal);
    }

    private void Update()
    {
        if(playing)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0f;

                playing = false;

                starsController.ActivateMenu(true);
            }

            starsController.UpdateTimer(timer);
        }
        
        if(missionsState == MissionsState.Final)
        {
            playing = false;

            starsController.ActivateMenu(true);
        }
    }

    // ----------------
    
    public void ChargePoints(int points)
    {
        actualPoints += points;

        CalculatePercentage();

        // Percentage Bar:
        starsController.UpdateProgressBar(points);
    }

    // ----------------

    private void CalculatePercentage()
    {
        if (actualPoints >= firstGoal && missionsState == MissionsState.none)
        {
            starsController.ActivateStar(0);
            missionsState = MissionsState.First;
        }
        else if (actualPoints >= mediumGoal && missionsState == MissionsState.First)
        {
            starsController.ActivateStar(1);
            missionsState = MissionsState.Medium;
        }
        else if (actualPoints >= finalGoal && missionsState == MissionsState.Medium)
        {
            starsController.ActivateStar(2);
            missionsState = MissionsState.Final;
        }
    }
}