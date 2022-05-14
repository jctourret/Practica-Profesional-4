using System.Collections;
using UnityEngine;

public class EliminateByTime : MonoBehaviour
{
    public enum Type
    {
        Furniture,
        Ornaments
    }

    [SerializeField] private Type objectType;

    [SerializeField] private GameObject[] fragments;

    private float secondsToDestroy = 10f;
    private float speedAnimation = 1f;

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

                if(fragments.Length > 0)
                    StartCoroutine(DestroyAnimation());
            }
        }
    }

    IEnumerator DestroyAnimation()
    {
        float time = 0f;

        //Vector3 initialScale = this.transform.localScale;
        Vector3 oneScale = Vector3.one;
        Vector3 zeroScale = Vector3.zero;

        while(time <= 1f)
        {
            time += Time.deltaTime;

            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i].transform.localScale = Vector3.Lerp(oneScale, zeroScale, time);
            }

            //this.transform.localScale = Vector3.Lerp(initialScale, zeroScale, time);

            yield return null;
        }

        Destroy(this.gameObject);
    }
    
    // ------------------------------------
    // Publics Setters / Getters:
    
    //public Type GetType()
    //{
    //    return objectType;
    //}

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