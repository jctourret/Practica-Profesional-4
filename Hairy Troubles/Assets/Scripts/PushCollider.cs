using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PushCollider : MonoBehaviour
{
    [SerializeField] float pushTime = 0.25f;
    [SerializeField] float pushCountdown;
    [SerializeField] float pushForce;
    [SerializeField] float upForce;
    Transform parent;
    bool canPush = false;
    private void OnEnable()
    {
        Movement.IsPushing += CanPush;
    }
    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if( pushCountdown >0 )
        {
            pushCountdown -= Time.deltaTime;
        }
        else
        {
            canPush = false;
        }
    }
    private void OnDisable()
    {
        Movement.IsPushing -= CanPush;
    }
    private void OnTriggerStay(Collider other)
    {
        if (canPush)
        {
            Rigidbody rb;

            other.gameObject.TryGetComponent<Rigidbody>(out rb);

            if (rb != null)
            {
                rb.AddForce(new Vector3( parent.forward.x * pushForce, upForce, parent.forward.z * pushForce), ForceMode.Impulse);
            }
        }
    }
   
    void CanPush()
    {
        canPush = true;
        pushCountdown = pushTime;
    }
}
