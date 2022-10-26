using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    #region EXPOSED_FIELD
    [SerializeField] protected Collider hazardCollider = null;
    [SerializeField] protected StatusEffects.StatesEnum effect = StatusEffects.StatesEnum.Burning;
    [SerializeField] [Range(1, 10)] protected float timerEffect = 3f;
    #endregion

    #region UNITY_CALLS
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out StatusEffects player))
        {
            switch (effect)
            {
                case StatusEffects.StatesEnum.Burning:
                    player.BurningState(timerEffect);
                    break;
                case StatusEffects.StatesEnum.UpsideDown:
                    player.UpsideDownState(timerEffect);
                    break;
                case StatusEffects.StatesEnum.Trapped:
                    player.TrappedState(timerEffect);
                    break;
            }

        }
    }
    #endregion

    #region VIRTUAL_CALLS
    protected virtual void TriggerEvent()
    {

    }
    #endregion
}