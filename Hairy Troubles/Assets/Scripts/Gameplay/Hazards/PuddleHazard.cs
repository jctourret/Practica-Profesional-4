using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleHazard : Hazard
{
    #region EXPOSED_FIELD
    [SerializeField] [Range(0, 180)] private float throwAngleX = 70;
    [SerializeField] private float throwForce = 130;
    #endregion

    #region PRIVATE_FIELD
    private const int ANGLE_DEGREES_Y = 360;
    #endregion

    #region UNITY_CALLS
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            int degreesY = Random.Range(0, ANGLE_DEGREES_Y);

            Vector3 direction = Quaternion.Euler(-throwAngleX, degreesY, 0) * Vector3.forward;

            player.UpsideDownState(throwForce, direction, timerEffect);
        }
    }
    #endregion
    
    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion

    #region PRIVATE_CALLS

    #endregion
}