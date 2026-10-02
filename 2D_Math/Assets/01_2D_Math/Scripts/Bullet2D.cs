using UnityEngine;

namespace Lesson2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet2D : MonoBehaviour
    {
        [Header("총알 속도")]
        [SerializeField] private float bulletSpeed = 10f; // [1]

        [Header("총알 설정")]
        public bool isEnemyBullet = false; // 보스가 쏜 총알인지 여부
        public int damage = 1;             // 총알 데미지

        private Rigidbody2D _rb2d;

        void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>(); // [1]
            // 기존 발사 방향(오른쪽)으로 속도 설정 [1]
            _rb2d.linearVelocity = transform.right * bulletSpeed;

            // [추가 기능] 생성된 후 5초가 지나면 자동으로 소멸
            Destroy(gameObject, 5.0f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 체력 시스템(Health) 컴포넌트가 있는지 확인
            Health targetHealth = collision.GetComponent<Health>();

            if (targetHealth != null)
            {
                // 보스 총알이 플레이어를 맞췄을 때
                if (isEnemyBullet && collision.CompareTag("Player"))
                {
                    targetHealth.TakeDamage(damage);
                    Destroy(gameObject); // 맞춘 후 총알 소멸
                }
                // 플레이어 총알이 보스를 맞췄을 때
                else if (!isEnemyBullet && collision.CompareTag("Boss"))
                {
                    targetHealth.TakeDamage(damage);
                    Destroy(gameObject); // 맞춘 후 총알 소멸
                }
            }
        }
    }
}