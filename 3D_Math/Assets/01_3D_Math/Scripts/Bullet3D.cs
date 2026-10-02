using UnityEngine;

namespace Lesson3D
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet3D : MonoBehaviour
    {
        [Header("총알 속도")]
        [SerializeField] private float bulletSpeed = 10f;

        private Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _rb.linearVelocity = transform.right * bulletSpeed;
        }
    }
}