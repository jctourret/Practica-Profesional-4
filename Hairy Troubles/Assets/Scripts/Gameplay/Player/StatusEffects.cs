using System;
using System.Collections;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public enum StatesEnum
    {
        Burning,
        UpsideDown,
        Trapped
    }

    #region EXPOSED_FIELD
    [SerializeField] private Movement playerMovement = null;
    #endregion

    #region PUBLIC_CALLS
    public void BurningState(float time)
    {
        StartCoroutine(State(time,
        () =>
        {
            playerMovement.IsMoving = false;
            playerMovement.IsDirectionBlocked = true;
        },
        (time) =>
        {
            Debug.Log("BURNING");
        },
        () =>
        {
            playerMovement.IsMoving = true;
            playerMovement.IsDirectionBlocked = false;
        }));
    }

    public void UpsideDownState(float time)
    {
        IEnumerator UpsideDown()
        {
            Debug.Log("UPSIDE DOWN");
            yield return null;
        }

        StartCoroutine(UpsideDown());
    }

    public void TrappedState(float time)
    {
        StartCoroutine(State(time, 
        () => 
        {
            playerMovement.StopPlayerInertia();
            playerMovement.IsMoving = false;
        },
        (time) =>
        { 
            Debug.Log("TRAPPED");
        }, 
        () => 
        {
            playerMovement.IsMoving = true;            
        }));
    }
    #endregion

    #region PRIVATE_CALLS
    private IEnumerator State(float time, Action start, Action<float> update, Action end)
    {
        float timer = 0;

        start?.Invoke();

        while (timer < time)
        {
            update?.Invoke(timer);

            timer += Time.deltaTime;
            yield return null;
        }

        end?.Invoke();
    }
    #endregion
}
