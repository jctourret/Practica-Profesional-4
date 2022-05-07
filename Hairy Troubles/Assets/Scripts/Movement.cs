using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour

{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpforce;
    [SerializeField] float pushforce;
    [SerializeField] float pushCooldown;
    [SerializeField] float pushCountdown;
    [SerializeField] Rigidbody rb;
    public static Action IsPushing;
    //[SerializeField] BoxCollider pushCollider;
    float hor;
    float ver;

    Vector3 movementDirection;
    bool canJump = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //pushCollider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        PlayerJumpLogic();
        
        PlayerPushLogic();

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
        //TARO: hace la logica para que esto se haga cuando choque con un suelo.
        canJump = true;
    }
    private void PlayerMovement()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        movementDirection = new Vector3(hor, 0, ver);
        movementDirection.Normalize();
    }
    private void PlayerJumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector3(0, jumpforce, 0), ForceMode.Impulse);
            canJump = false;
        }
    }
    private void PlayerPushLogic()
    {
        if (pushCountdown >= 0)
        {
            pushCountdown -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsPushing?.Invoke();
            // rb.AddForce(transform.forward * pushforce, ForceMode.Acceleration);
            pushCountdown = pushCooldown;
        }
    }
}
