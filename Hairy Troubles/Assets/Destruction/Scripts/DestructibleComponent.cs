using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleComponent : MonoBehaviour
{
    [Header("Fractured Part")]
    [SerializeField] protected GameObject fracturedObj;

    [Header("Particle")]
    [SerializeField] protected GameObject particlePref;

    protected Rigidbody rig;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;

    protected float velocity;

    protected virtual void Awake()
    {
        rig = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    protected void SwapComponent()
    {
        meshRenderer.enabled = false;
        meshCollider.enabled = false;

        rig.isKinematic = true;

        if (fracturedObj != null)
        {
            fracturedObj.SetActive(true);
        }

        if(particlePref != null)
        {
            GameObject go = Instantiate(particlePref, this.transform.position, Quaternion.Euler(Vector3.up));

            Destroy(go, 3f);
        }
    }
}
