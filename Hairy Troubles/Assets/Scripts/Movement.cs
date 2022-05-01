using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour

{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpfoce;
    [SerializeField] float cos;
    [SerializeField] Rigidbody rb;
    float hor;
    float ver;

    Vector3 movementDirection;
    bool canJump = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        movementDirection = new Vector3(hor, 0, ver);
        movementDirection.Normalize();

        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector3(0, jumpfoce, 0), ForceMode.Impulse);
            canJump = false;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(hor * movementSpeed, rb.velocity.y, ver * movementSpeed);

        if (movementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movementDirection , Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }
}
