using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleComponent : MonoBehaviour
{
    [Header("Fractured Part")]
    [SerializeField] protected GameObject fracturedObj;

    protected Rigidbody rig;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;

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

        fracturedObj.SetActive(true);
    }
}
