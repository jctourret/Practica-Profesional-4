using UnityEngine;
using System;

public class DestructibleComponent : MonoBehaviour
{
    public static event Action<int> InScenePoints;
    public static event Action<int> DestructionPoints;

    [Header("Fractured Part")]
    [SerializeField] protected GameObject fracturedObj;
    [SerializeField] protected int points = 10;

    [Header("Particle")]
    [SerializeField] protected GameObject particlePref;

    protected Rigidbody rig;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;

    protected float velocity;

    protected bool isDestroyed = false;

    protected virtual void Awake()
    {
        rig = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        InScenePoints?.Invoke(points);
    }

    protected void SwapComponent()
    {
        if (!isDestroyed)
        {
            meshRenderer.enabled = false;
            meshCollider.enabled = false;

            rig.isKinematic = true;

            DestructionPoints?.Invoke(points);
            isDestroyed = true;
            
            if (fracturedObj != null)
            {
                fracturedObj.SetActive(true);
            }

            if (particlePref != null)
            {
                GameObject go = Instantiate(particlePref, this.transform.position, Quaternion.Euler(Vector3.up));

                Destroy(go, 3f);
            }
        }
    }
}
