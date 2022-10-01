using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour, ICollidable
{
    public static Action<float, float, float> IsPushing;
    public static Action onHighlightRequest;
    public static Action OnBerserkModeEnd;

    [Space(10f)]
    [Header("-- Movement --")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float pushforce;
    [SerializeField] private float pushCooldown;
    [SerializeField] private float pushCountdown;
    [SerializeField] private float positionY;
    [SerializeField] private List<Transform> raycast;

    [SerializeField] private Animator anim;
    [Space(10f)]
    [Header("-- Push --")]
    [Space(20f)]
    [Range(0.01f, 1f)]
    [SerializeField] private float pushTime = 0.25f;
    [SerializeField] private float frontForce = 1;
    [SerializeField] private float upForce = 1;

    [Header("-- Berserk Mode --")]
    [SerializeField] private Renderer bodyRend;
    [SerializeField] private Renderer eyesRend;
    [SerializeField] private float duration;
    [SerializeField] private float jumpForceBuff = 1;
    [SerializeField] private float scaling = 2;
    [SerializeField] private float scalingSpeed = 1;
    [SerializeField] private Color tint;
    [SerializeField] private float tintChangeDelay = 1;

    private float hor;
    private float ver;
    private Vector3 movementDirection;
    private bool canJump = false;
    private bool isMoving = true;
    private bool berserkMode;

    private Rigidbody rb;

    // ----------------------

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        GameManager.OnComboBarFull += EnterBerserkMode;
    }

    private void OnDisable()
    {
        GameManager.OnComboBarFull -= EnterBerserkMode;
    }
    void Update()
    {
        if (isMoving)
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");

            movementDirection = new Vector3(hor, 0, ver);
            movementDirection.Normalize();

            PlayerJumpLogic();

            PlayerHighlightRequest();

            PlayerPushLogic();

            //PlayerMovement();
        }
    }

    void PlayerHighlightRequest()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            onHighlightRequest?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            PlayerMovement();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        
        for(int i = 0; i < raycast.Count; i++)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raycast[i].transform.position, raycast[i].transform.TransformDirection(Vector3.down), out hit, transform.localScale.y / 20.0f))
            {
                canJump = true;
                break;
            }
        }
    }

    private void PlayerMovement()
    {
        rb.velocity = new Vector3(hor * movementSpeed, rb.velocity.y, ver * movementSpeed);
        
        anim.SetInteger("MovementVector", (int)movementDirection.magnitude);

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

        if (positionY > transform.position.y)
        {
            anim.SetTrigger("Fall");
        }

        positionY = transform.position.y;
    }

    private void PlayerPushLogic()
    {
        if (pushCountdown >= 0)
        {
            pushCountdown -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Push");
            IsPushing?.Invoke(pushTime, frontForce, upForce);
            pushCountdown = pushCooldown;
        }
    }

    private void EnterBerserkMode()
    {
        if (!berserkMode)
        {
            berserkMode = true;
            StartCoroutine(BerserkMode());
        }
    }

    // ------------------------------


    public void StopCharacter(bool state)
    {
        isMoving = state;
    }


    IEnumerator BerserkMode()
    {
        float timer = 0;
        Color startColor = bodyRend.material.GetColor("_MainColor");
        Vector3 startScale = transform.localScale;
        Vector3 endScale = transform.localScale*2.0f;
        jumpforce += jumpForceBuff;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bodyRend.material.SetColor("_MainColor", Color.Lerp(bodyRend.material.GetColor("_MainColor"), tint, timer / duration));
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, timer / duration);
            yield return null;
        }
        OnBerserkModeEnd?.Invoke();
        jumpforce -= jumpForceBuff;
        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bodyRend.material.SetColor("_MainColor", Color.Lerp(bodyRend.material.GetColor("_MainColor"), startColor, timer / duration));
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, timer / duration);
            yield return null;
        }
        berserkMode = false;
    }
}
