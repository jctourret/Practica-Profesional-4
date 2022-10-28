using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenHazard : Hazard
{
    #region EXPOSED_FIELD
    [SerializeField] [Range(0,10)] private float activeTime = 3f;
    [SerializeField] [Range(0,10)] private float timeToActivate = 3f;

    [SerializeField] private OvenFire[] ovenParticles;
    #endregion

    #region PRIVATE_FIELD
    private float ovenTime = 0f;
    private bool active = false;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        for (int i = 0; i < ovenParticles.Length; i++)
        {
            ovenParticles[i].Init();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            player.BurningState(timerEffect);
        }
    }

    private void Update()
    {
        ovenTime += Time.deltaTime;

        if (active)
        {
            if (ovenTime > activeTime)
            {
                active = false;
                EnableCollider(active);

                SwapParticles(active);

                ovenTime = 0;
            }
        }
        else
        {
            if (ovenTime > timeToActivate)
            {
                active = true;
                EnableCollider(active);

                SwapParticles(active);

                ovenTime = 0;
            }
        }
    }
    #endregion

    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion

    #region PRIVATE_CALLS
    private void SwapParticles(bool state)
    {
        for (int i = 0; i < ovenParticles.Length; i++)
        {
            ovenParticles[i].SwapActiveParticle(state);
        }
    }
    #endregion
}