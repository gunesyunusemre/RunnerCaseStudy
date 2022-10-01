using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class RigidbodyFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;
        
        
        private void FixedUpdate()
        {
            //var horizontal = Input.GetAxis("Horizontal");
            var pos = target.position;
            var targetPos = Vector3.Lerp(rb.position, pos, Time.fixedDeltaTime * speed);
            rb.MovePosition(targetPos);
        }
    }
}