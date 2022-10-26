using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour, ICollidable
{
    #region EXPOSED_METHODS
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private AudioSource footstepsSFX;
    [SerializeField] private AudioSource SFX;
    [SerializeField] private Animator anim = null;
    [Space(10f)]
    [Header("-- Movement --")]
    [SerializeField] private float movementSpeed = 1.75f;
    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private float jumpforce = 14;
    [SerializeField] private float pushforce = 7;
    [SerializeField] private float pushCooldown = 1;
    [SerializeField] private float pushCountdown = 0;
    [SerializeField] private float positionY = 0;
    [SerializeField] private List<Transform> raycast;

    [Header("-- Push --")]
    [Range(0.01f, 1f)]
    [SerializeField] private float pushTime = 0.25f;
    [SerializeField] private float frontForce = 1;
    [SerializeField] private float upForce = 1;

    [Header("-- Berserk Mode --")]
    [SerializeField] private Renderer bodyRend = null;
    [SerializeField] private Renderer eyesRend = null;

    [SerializeField] private float duration = 10;
    [SerializeField] private float jumpForceBuff = 10.5f;
    [SerializeField] private float scaling = 2;
    [SerializeField] private float scalingSpeed = 1;
    [SerializeField] private Color tint = Color.red;
    [SerializeField] private float tintChangeDelay = 1;

    [Header("-- Particles --")]
    [SerializeField] private ParticleSystem dustTrail = null;
    [SerializeField] private ParticleSystem slamDustTrail = null;
    #endregion

    #region PRIVATE_METHODS
    private Rigidbody rb = null;

    private Vector3 movementDirection = Vector3.zero;
    private float hor = 0;
    private float ver = 0;
    
    private bool canJump = true;
    private bool isMoving = true;
    enum PlayerAction
    {
        push,
        jump
    }
    PlayerAction playerAction;
    private bool isDirectionBlocked = false;
    private bool berserkMode = false;
    #endregion

    #region PROPERTIES
    public bool CanJump { get => canJump; set => canJump = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public bool IsDirectionBlocked { get => isDirectionBlocked; set => isDirectionBlocked = value; }
    #endregion

    #region ACTIONS
    public static Action<float, float, float> IsPushing;
    public static Action onHighlightRequest;
    public static Action OnBerserkModeEnd;
    #endregion

    #region UNITY_CALLS
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

            if (movementDirection != Vector3.zero)
            {
                if (!footstepsSFX.isPlaying)
                    footstepsSFX.Play();
            }
            else
                footstepsSFX.Stop();
            PlayerJumpLogic();

            PlayerHighlightRequest();

            PlayerPushLogic();
        }
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            PlayerMovement();
        }
        else if (isDirectionBlocked)
        {
            BlockedMovement();
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void StopPlayerInertia()
    {
        rb.velocity = Vector3.zero;
    }

    public void BlockMovement()
    {

    }
    #endregion

    #region PRIVATE_CALLS
    private void PlayerHighlightRequest()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            onHighlightRequest?.Invoke();
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
                if(!canJump)
                {
                    slamDustTrail.Play();//.gameObject.SetActive(true);
                }

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
            dustTrail.gameObject.SetActive(true);

            Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
        }
        else
        {
            dustTrail.gameObject.SetActive(false);
        }
    }

    private void BlockedMovement()
    {
        dustTrail.gameObject.SetActive(true);

        rb.velocity = new Vector3(transform.forward.x * movementSpeed, rb.velocity.y, transform.forward.z * movementSpeed);
        anim.SetInteger("MovementVector", (int)transform.forward.magnitude);

        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        rb.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }

    private void PlayerJumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump && isMoving)
        {
            rb.AddForce(new Vector3(0, jumpforce, 0), ForceMode.Impulse);
            canJump = false;
            SFX.PlayOneShot(audioClips[(int)PlayerAction.jump]);
            //slamDustTrail.gameObject.SetActive(canJump);
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
            SFX.PlayOneShot(audioClips[(int)PlayerAction.push]);
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

    private IEnumerator BerserkMode()
    {
        float timer = 0;
        Color startColor = bodyRend.material.GetColor("_MainColor");
        Vector3 startScale = transform.localScale;
        Vector3 endScale = transform.localScale * 2.0f;
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
    #endregion    
}