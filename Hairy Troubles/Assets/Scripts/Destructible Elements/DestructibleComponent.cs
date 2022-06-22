using UnityEngine;
using System;
using System.Collections;
public class DestructibleComponent : MonoBehaviour
{
    //public static event Action<int> InScenePoints;
    public static event Action<int> DestructionPoints;

    [Header("Fractured Part")]
    [SerializeField] protected GameObject fracturedObj;
    [SerializeField] protected int points = 10;

    [Header("Particle")]
    [SerializeField] protected GameObject particlePref;

    [Header("Highlight")]
    protected Renderer renderer;
    protected Color startingColor;
    protected Color highlightColor = Color.red;
    [SerializeField] protected float highlightTime = 3f;
    [SerializeField] protected float highlightTimer=0f;


    [Header("Destruction")]
    [SerializeField] float destructionTime = 3f;

    protected Rigidbody rig;
    protected MeshRenderer meshRenderer;
    protected Collider meshCollider;

    protected float velocity;

    protected bool isDestroyed = false;

    protected virtual void Awake()
    {
        rig = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        GameManager.static_scenePoints += points;
        Movement.onHighlightRequest += startHighlight;
    }
    private void OnDisable()
    {
        Movement.onHighlightRequest -= startHighlight;
    }

    void startHighlight()
    {
        startingColor = renderer.material.GetColor("Color_b9a9dcbfa87e4092934e4e9c37497682");
        renderer.material.SetColor("Color_b9a9dcbfa87e4092934e4e9c37497682",highlightColor);
        StartCoroutine(HighlightItems());
    }

    protected void SwapComponent()
    {
        if (!isDestroyed)
        {
            meshRenderer.enabled = false;
            meshCollider.enabled = false;

            rig.isKinematic = true;

            DestructionPoints?.Invoke(points);
            isDestroyed = true;
            
            if (fracturedObj != null)
            {
                fracturedObj.SetActive(true);
            }

            if (particlePref != null)
            {
                GameObject go = Instantiate(particlePref, this.transform.position, Quaternion.Euler(Vector3.up));

                Destroy(go, destructionTime);
            }
        }
    }

    IEnumerator HighlightItems()
    {
        while (highlightTimer < highlightTime)
        {
            highlightTimer += Time.deltaTime;
            yield return null;
        }
        highlightTimer = 0.0f;
        renderer.material.SetColor("Color_b9a9dcbfa87e4092934e4e9c37497682", startingColor);
    }
}
