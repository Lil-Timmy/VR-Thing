using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterWalking : MonoBehaviour
{
    [SerializeField] float frequency;
    [SerializeField] float amplitude;
    
    void FixedUpdate()
    {
        transform.position += Vector3.forward * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
