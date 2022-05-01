using UnityEngine;

public class FurnitureParts : DestructibleComponent
{
    [Header("Furniture Part")]
    public bool isDestoyed = false;
    [Range(1, 20)]
    //[SerializeField] private float timeToDestroy = 5f;

    private FixedJoint fixedJoint;

    // -------------------------------

    protected override void Awake()
    {
        base.Awake();

        fixedJoint = GetComponent<FixedJoint>();
    }

    void FixedUpdate()
    {
        velocity = rig.velocity.magnitude;
    }

    public void CollapseComponent()
    {

    }

    // -------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (fixedJoint != null) Destroy(fixedJoint);
            SwapComponent();
        }
    }
}