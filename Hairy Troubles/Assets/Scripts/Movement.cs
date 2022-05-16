using System.Collections;
using UnityEngine;
using System;

public class Movement : MonoBehaviour, ICollidable
{
    public static Action<float, float, float> IsPushing;
    
    [Space(10f)]
    [Header("-- Movement --")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float pushforce;
    [SerializeField] private float pushCooldown;
    [SerializeField] private float pushCountdown;
    [SerializeField] private Animator anim;
    [Space(10f)]
    [Header("-- Push --")]
    [Space(20f)]
    [Range(0.01f, 1f)]
    [SerializeField] private float pushTime = 0.25f;
    [SerializeField] private float frontForce = 1;
    [SerializeField] private float upForce = 1;

    private float hor;
    private float ver;
    private Vector3 movementDirection;
    private bool canJump = false;

    private Rigidbody rb;

    // ----------------------

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMovement();

        PlayerJumpLogic();
        
        PlayerPushLogic();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TARO: hace la logica para que esto se haga cuando choque con un suelo.
        canJump = true;
        //COMENTARIO - TOMAS: No me parece muy copada la forma en la que se chequea la colision despues de un salto usando el "OnCollisionEnter"
    }

    private void PlayerMovement()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        movementDirection = new Vector3(hor, 0, ver);
        movementDirection.Normalize();

        anim.SetInteger("MovementVector", (int)movementDirection.magnitude);

        rb.velocity = new Vector3(hor * movementSpeed, rb.velocity.y, ver * movementSpeed);

        if (movementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
        }
    }

    private void PlayerJumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector3(0, jumpforce, 0), ForceMode.Impulse);
            canJump = false;
            anim.SetTrigger("Jump");
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
            IsPushing?.Invoke(pushTime, frontForce, upForce);
            pushCountdown = pushCooldown;
        }
    }
}
