using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollision : DestructibleComponent
{
    [Header("Particular Object")]
    [Range(0.01f, 20f)]
    [SerializeField] private float fractureLimit = 2.0f;

    // -------------------------------

    void FixedUpdate()
    {
        velocity = rig.velocity.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(velocity <= -fractureLimit)
        {
            SwapComponent();
        }
    }

}
