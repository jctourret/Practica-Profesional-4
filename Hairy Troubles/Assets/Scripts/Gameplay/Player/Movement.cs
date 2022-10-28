using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour, ICollidable
{
    #region EXPOSED_METHODS
    [Space(10f)]
    [Header("-- Movement --")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float smoothRotation;
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
    [Space(10f)]
    [Header("-- Grab --")]
    [SerializeField] private float springForce = 1000;
    [SerializeField] private float pushDragThreshold = 20f;
    [SerializeField] private Transform anchorPoint;
    bool invertMovement;

    [Space(10f)]
    [Header("-- Berserk Mode --")]
    [SerializeField] private Renderer bodyRend;
    [SerializeField] private Renderer eyesRend;
    [SerializeField] private float duration;
    [SerializeField] private float jumpForceBuff = 1;
    [SerializeField] private Color tint;

    [Header("-- Particles --")]
    [SerializeField] private ParticleSystem dustTrail = null;
    [SerializeField] private ParticleSystem slamDustTrail = null;
    #endregion

    #region PRIVATE_METHODS
    private Rigidbody rb;

    private Vector3 movementDirection;
    private float hor;
    private float ver;
    private float yVelocity;
    
    private bool canJump = true;
    private bool isMoving = true;
    private bool berserkMode;
    #endregion

    #region ACTIONS
    public static Action<float, float, float> IsPushing;
    public static Action OnGrab;
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
        PushCollider.OnObjectGrabbed += RecieveGrabbed;
    }

    private void OnDisable()
    {
        GameManager.OnComboBarFull -= EnterBerserkMode;
        PushCollider.OnObjectGrabbed -= RecieveGrabbed;
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

            PlayerGrabLogic();

            PlayerPushLogic();
        }
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            PlayerMovement();
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void StopCharacter(bool state)
    {
        isMoving = state;
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

        if (invertMovement)
        {
            rb.velocity = new Vector3(-hor * movementSpeed, rb.velocity.y, -ver * movementSpeed);
        }
        else
        {
            rb.velocity = new Vector3(hor * movementSpeed, rb.velocity.y, ver * movementSpeed);
        }  
        anim.SetInteger("MovementVector", (int)movementDirection.magnitude);

        if (movementDirection != Vector3.zero)
        {
            dustTrail.gameObject.SetActive(true);
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref yVelocity, smoothRotation);
            rb.rotation = Quaternion.Euler(0f,angle,0f);
            //Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            //rb.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
        }
        else
        {
            dustTrail.gameObject.SetActive(false);
        }
    }

    private void PlayerJumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector3(0, jumpforce, 0), ForceMode.Impulse);
            canJump = false;
            //slamDustTrail.gameObject.SetActive(canJump);
            anim.SetTrigger("Jump");
        }

        if (positionY > transform.position.y)
        {
            anim.SetTrigger("Fall");
        }

        positionY = transform.position.y;
    }

    private void PlayerGrabLogic()
    {
        SpringJoint joint;
        gameObject.TryGetComponent<SpringJoint>(out joint);
        if(joint != null)
        {
            joint.connectedBody.gameObject.TryGetComponent<DestroySetsOfComponents>(out DestroySetsOfComponents sets);
            joint.connectedBody.gameObject.TryGetComponent<MeshCollider>(out MeshCollider mesh);
            if ((mesh !=null &&!mesh.enabled) || joint.connectedBody == null || (sets!=null && sets.groupDestroyed))
            {
                Destroy(joint);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(joint == null)
            {
                OnGrab?.Invoke();
            }
            else
            {
                if (joint != null)
                {
                    Rigidbody grabbed = joint.connectedBody;
                    Destroy(joint);
                    Debug.Log("Throwing Grabbed Object");
                    grabbed.AddForce(new Vector3(transform.forward.x * frontForce, upForce, transform.forward.z * frontForce), ForceMode.Impulse);
                    invertMovement = false;
                }
            }
        }
    }

    private void RecieveGrabbed(Rigidbody grabbedObject)
    {
        if (!gameObject.GetComponent<SpringJoint>())
        {
            SpringJoint joint = gameObject.AddComponent<SpringJoint>();
            joint.anchor = transform.InverseTransformPoint(anchorPoint.position);
            joint.spring = springForce;
           
            joint.connectedBody = grabbedObject;
            if (grabbedObject.mass >= pushDragThreshold)
            {
                invertMovement = true;
            }
            else
            {
                invertMovement = false;
            }
        }
        Debug.Log("Recieved grabbable");
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