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
        (t) => { },
        () =>
        {
            playerMovement.IsMoving = true;
            playerMovement.IsDirectionBlocked = false;
        }));
    }

    public void UpsideDownState(float force, Vector3 direction, float time)
    {
        StartCoroutine(State(time,
        () =>
        {
            playerMovement.IsMoving = false;
            PlayerThrow(force, direction);
        },
        (t) => { },
        () =>
        {
            playerMovement.IsMoving = true;
        }));
    }

    public void TrappedState(float time)
    {
        StartCoroutine(State(time, 
        () => 
        {
            playerMovement.StopPlayerInertia();
            playerMovement.IsMoving = false;
        },
        (t) => { }, 
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

    private void PlayerThrow(float force, Vector3 direction)
    {
        playerMovement.Rb.velocity = Vector3.zero;
        playerMovement.Rb.AddForce(direction * force, ForceMode.Acceleration);
    }
    #endregion
}
