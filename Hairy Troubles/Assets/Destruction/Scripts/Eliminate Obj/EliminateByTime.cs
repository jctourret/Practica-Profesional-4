using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateByTime : MonoBehaviour
{
    public enum Type
    {
        Furniture,
        Ornaments
    }

    [SerializeField] private Type objectType;

    [SerializeField] private float secondsToDestroy = 10f;

    [Range(0.001f, 2f)]
    [SerializeField] private float speedAnimation = 1f;

    private bool animate = true;
    private float time = 0f;

    // -----------------------

    private void OnEnable()
    {
        secondsToDestroy = Control_Obj_Eliminations.Get().GetSeconds(objectType);
        speedAnimation = Control_Obj_Eliminations.Get().GetSpeed(objectType);
        
        animate = false;
    }

    private void FixedUpdate()
    {
        if (!animate)
        {
            time += Time.deltaTime;

            if (time > secondsToDestroy)
            {
                animate = true;
                StartCoroutine(DestroyAnimation());
            }
        }
    }

    IEnumerator DestroyAnimation()
    {
        float time = 0f;

        Vector3 initialScale = this.transform.localScale;
        Vector3 zeroScale = Vector3.zero;

        while(time <= 1f)
        {
            time += Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(initialScale, zeroScale, time);

            yield return null;
        }

        Destroy(this);
    }
    
    // ------------------------------------
    // Publics Setters / Getters:
    
    public Type GetType()
    {
        return objectType;
    }

    public void SetSecondsToDestroy(float seconds)
    {
        secondsToDestroy = seconds;
    }

    public void SetSpeedAnimation(float speed)
    {
        speedAnimation = speed;
    }

    public float GetSecondsToDestroy(float seconds)
    {
        return secondsToDestroy;
    }

    public float GetSpeedAnimation(float speed)
    {
        return speedAnimation ;
    }
}