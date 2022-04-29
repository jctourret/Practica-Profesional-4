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

    [Header("UI")]
    [SerializeField] private MissionsState missionsState;

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

    private UI_StarPoints_Controller starsController;

    // ----------------

    private void Awake()
    {
        starsController = GetComponent<UI_StarPoints_Controller>();

        missionsState = MissionsState.none;
    }

    private void Start()
    {
        firstGoal = scenePoints * ((float)firstPercentGoal / PERCENT);
        mediumGoal = scenePoints * ((float)mediumPercentGoal / PERCENT);
        finalGoal = scenePoints * ((float)finalPercentGoal / PERCENT);

        Debug.Log(firstPercentGoal + "% percent: " + firstGoal);
        Debug.Log(mediumPercentGoal + "% percent: " + mediumGoal);
        Debug.Log(finalPercentGoal + "% percent: " + finalGoal);
    }

    private void OnEnable()
    {
        DestructibleComponent.InScenePoints += GoalPoints;
        DestructibleComponent.DestructionPoints += ChargePoints;
    }

    private void OnDisable()
    {
        DestructibleComponent.InScenePoints -= GoalPoints;
        DestructibleComponent.DestructionPoints -= ChargePoints;        
    }

    // ----------------

    public void GoalPoints(int points)
    {
        scenePoints += points;
    }
    
    public void ChargePoints(int points)
    {
        actualPoints += points;

        CalculatePercentage();
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