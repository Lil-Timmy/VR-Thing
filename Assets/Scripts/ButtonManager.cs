using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] float pushBackForce = 100f;
    [SerializeField] float pushBackLimit = 0.1f;

    Rigidbody rb;
    Vector3 targetPos;

    void Start()
    {
        targetPos = transform.localPosition;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 buttonVelocity = (targetPos - transform.localPosition) * pushBackForce;
        rb.velocity = buttonVelocity;
    }

    void Update()
    {
        transform.localPosition = new Vector3(targetPos.x, Mathf.Clamp(transform.localPosition.y, targetPos.y - pushBackLimit, targetPos.y), targetPos.z);
    }
}
