using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region PUBLIC_METHODS
    public static Action OnComboBarFull;

    public enum MissionsState
    {
        none, First, Medium, Final
    }
    
    [Header("Scene Points")]
    public float scenePoints = 0;
    public float actualPoints = 0;

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

    [Header("Player Ref")]
    [SerializeField] private Movement player = null;

    [Header("--- COMBO ---")]
    [SerializeField] private int targetDestructibles;

    [Header("UI")]
    [SerializeField] private UiGameController uiGameController;
    #endregion

    #region PRIVATE_METHODS
    private float firstGoal = 0f;
    private float mediumGoal = 0f;
    private float finalGoal = 0f;

    private float timer = 0f;
    private bool playing = true;
    #endregion

    #region STATIC_CONST_METHODS
    public static float static_scenePoints = 0f;
    private const float PERCENT = 100f;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        missionsState = MissionsState.none;

        timer = sceneTime;

        scenePoints = 0;
        actualPoints = 0;

        uiGameController.Initialize();
    }

    private void OnEnable()
    {
        //DestructibleComponent.InScenePoints += GoalPoints;
        DestructibleComponent.OnDestruction += ChargePoints;
        DestructibleComponent.OnDestruction += ChargeComboBar;
        Movement.OnBerserkModeEnd += UnlockComboBar;
    }

    private void OnDisable()
    {
        static_scenePoints = 0;

        //DestructibleComponent.InScenePoints -= GoalPoints;
        DestructibleComponent.OnDestruction -= ChargePoints;
        DestructibleComponent.OnDestruction -= ChargeComboBar;
        Movement.OnBerserkModeEnd -= UnlockComboBar;
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

        // UI:
        uiGameController.SetValues(finalGoal, ((float)firstPercentGoal / PERCENT), ((float)mediumPercentGoal / PERCENT), ((float)finalPercentGoal / PERCENT));
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
                player.StopCharacter(playing);

                CalculatePercentage();
                uiGameController.ActivateMenu(true);
            }

            uiGameController.UpdateTimer(timer);
        }

        if (Input.GetKeyDown(KeyCode.Q)&&starsController.CheckComboBar())
        {
            OnComboBarFull?.Invoke();
            starsController.SetDeclineLock(false);
        }

        if(missionsState == MissionsState.Final)
        {
            playing = false;
            player.StopCharacter(playing);

            uiGameController.ActivateMenu(true);
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void ChargePoints(int points)
    {
        actualPoints += points;

        CalculatePercentage();

        // Percentage Bar:
        uiGameController.UpdateProgressBar(points);
    }
    #endregion

    #region PRIVATE_CALLS
    // ----------------

    private void ChargeComboBar(int i)
    {
        starsController.ChargeComboBar(targetDestructibles);
    }

    private void UnlockComboBar()
    {
        starsController.SetGrowthLock(false);
    }

    private void CalculatePercentage()
    {
        if (actualPoints >= firstGoal && missionsState == MissionsState.none)
        {
            uiGameController.ActivateFinalStar(0);
            missionsState = MissionsState.First;
        }
        
        if (actualPoints >= mediumGoal && missionsState == MissionsState.First)
        {
            uiGameController.ActivateFinalStar(1);
            missionsState = MissionsState.Medium;
        }
        
        if (actualPoints >= finalGoal && missionsState == MissionsState.Medium)
        {
            uiGameController.ActivateFinalStar(2);
            missionsState = MissionsState.Final;
        }
    }
    #endregion
}