using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureParts : DestructibleComponent
{
    [Header("Furniture Part")]
    public bool isDestoyed = false;
    [Range(1, 20)]
    [SerializeField] private float timeToDestroy = 5f;

    private FixedJoint fixedJoint;

    // -------------------------------

    protected override void Awake()
    {
        base.Awake();

        fixedJoint = GetComponent<FixedJoint>();
    }

    //private void Awake()
    //{
    //    fixedJoint = GetComponent<FixedJoint>();
    //}

    public void CollapseComponent()
    {
        fixedJoint.connectedBody = null;

        //SwapComponent();
    }

    // -------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Destroy(fixedJoint);
            //fixedJoint.connectedBody = null;

            SwapComponent();
        }
    }
}
