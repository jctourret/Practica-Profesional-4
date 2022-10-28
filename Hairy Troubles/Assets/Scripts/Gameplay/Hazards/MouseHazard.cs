using System.Collections;
using UnityEngine;

public class MouseHazard : Hazard
{
    #region EXPOSED_FIELD
    [SerializeField][Range(0, 10)] private float reloadTime = 3f;
    #endregion

    #region PRIVATE_FIELD
    #endregion

    #region UNITY_CALLS
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            player.TrappedState(timerEffect);

            StartCoroutine(ReloadHazard());
        }
    }
    #endregion

    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion

    #region PRIVATE_CALLS
    private IEnumerator ReloadHazard()
    {
        float time = 0f;
        float finalTime = timerEffect + reloadTime;

        EnableCollider(false);

        while (time <= finalTime)
        {
            time += Time.deltaTime;

            yield return null;
        }

        EnableCollider(true);
    }
    #endregion
}