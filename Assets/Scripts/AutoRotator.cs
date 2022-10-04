using System;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    

    private void Awake()
    {
        if (target == null)
            target = transform;
        
    }

    private void Update()
    {
        target.Rotate(axis * (speed * Time.deltaTime), Space.Self);
    }
}