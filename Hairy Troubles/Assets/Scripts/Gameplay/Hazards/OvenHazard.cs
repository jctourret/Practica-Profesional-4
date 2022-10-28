using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenHazard : Hazard
{

    #region UNITY_CALLS
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            player.BurningState(timerEffect);
        }
    }
    #endregion

    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion
}