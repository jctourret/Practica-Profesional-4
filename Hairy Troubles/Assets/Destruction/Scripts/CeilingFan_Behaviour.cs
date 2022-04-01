using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFan_Behaviour : MonoBehaviour
{

    [SerializeField] private GameObject fanObj;

    [SerializeField] private Vector3 rotation;
    [Range(0.1f, 20)]
    [SerializeField] private float speed = 2f;

    private FixedJoint fixedJoint;
    private bool collapse = false;

    // -----------------------

    private void Awake()
    {
        fixedJoint = GetComponent<FixedJoint>();
    }

    private void Update()
    {
        InputCollapse();
        MoveFan();
    }

    private void InputCollapse()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !collapse)
        {
            collapse = true;
            Destroy(fixedJoint);
        }
    }

    private void MoveFan()
    {
        if(!collapse)
            fanObj.transform.Rotate(rotation * speed * Time.deltaTime);
    }

}
