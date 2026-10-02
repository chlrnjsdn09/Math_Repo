using UnityEngine;

namespace Lesson2D
{
    public class Health : MonoBehaviour
    {
        [Header("체력 설정")]
        [SerializeField] private int maxHealth = 5;
        private int _currentHealth;

        void Start()
        {
            _currentHealth = maxHealth;
        }

        /// <summary>
        /// 외부(총알 등)로부터 데미지를 입을 때 호출
        /// </summary>
        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.Log($"{gameObject.name}가 피격당했습니다! 남은 체력: {_currentHealth}/{maxHealth}");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name}가 소멸했습니다.");
            Destroy(gameObject);
        }
    }
}