using UnityEngine;

public class DestroyByCollision : DestructibleComponent, ICollidable
{
    public enum ObjSurface
    {
        Floor,
        Wall,
        Roof
    }

    [Header("Particular Object")]
    [Range(0.01f, 20f)]
    [SerializeField] private float fractureLimit = 2.0f;
    [Space(15f)]
    [Header("Features")]
    [SerializeField] private ObjSurface objSurface = ObjSurface.Floor;

    // -------------------------------

    void FixedUpdate()
    {
        velocity = rig.velocity.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(objSurface == ObjSurface.Floor)
        {
            if (velocity <= -fractureLimit)
            {
                SwapComponent();
            }
        }
        else if (objSurface == ObjSurface.Wall)
        {
            if (collision.transform.GetComponent<ICollidable>() != null)
            {
                rig.isKinematic = false;
                objSurface = ObjSurface.Floor;
            }
        }
        else if (objSurface == ObjSurface.Roof)
        {

        }
    }
}