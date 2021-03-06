using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCollider : MonoBehaviour
{
    private float pushTime = 0.25f;
    private float frontforce = 1;
    private float upForce = 1;
    
    private Transform parent;
    private Collider coll;

    private List<Transform> objectsPushed = new List<Transform>();

    // ----------------------

    void Awake()
    {
        parent = GetComponentInParent<Transform>();
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        coll.enabled = false;
    }

    private void OnEnable()
    {
        Movement.IsPushing += CanPush;
    }

    private void OnDisable()
    {
        Movement.IsPushing -= CanPush;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb;

        other.gameObject.TryGetComponent<Rigidbody>(out rb);

        if (rb != null && !objectsPushed.Contains(other.transform))
        {
            objectsPushed.Add(other.transform);

            rb.AddForce(new Vector3( parent.forward.x * frontforce, upForce, parent.forward.z * frontforce), ForceMode.Impulse);
        }
    }
   
    private void CanPush(float time, float front, float up)
    {
        pushTime = time;
        frontforce = front;
        upForce = up;

        StartCoroutine(PushCountdown());
    }

    IEnumerator PushCountdown()
    {
        coll.enabled = true;

        yield return new WaitForSeconds(pushTime);

        objectsPushed.Clear();
        coll.enabled = false;
    }
}
