using System.Collections;
using UnityEngine;
using EducationalMath;

namespace Lesson2D
{
    public enum MoveMode { None, Linear, Circle }
    public enum FireMode { None, Stream, Shotgun }

    public class BulletSpawner2D : MonoBehaviour
    {
        [Header("모드 선택")]
        public MoveMode moveMode = MoveMode.Linear;
        public FireMode fireMode = FireMode.Stream;

        [Header("이동")]
        public float moveSpeed = 5f;
        public float moveAngle = 30f;
        public float circleSpeed = 90f; // 초당 회전 각도

        [Header("총알 발사")]
        public GameObject bulletPrefab;
        public Transform bulletContainer;
        public float streamFireRate = 0.1f;
        public float shotgunCooldown = 3f;
        public int angleInterval = 10;
        public int startAngle = 30;
        public int endAngle = 330;

        private float _currentOrbitAngle = 0f;

        void Start()
        {
            switch (fireMode)
            {
                case FireMode.Stream:
                    StartCoroutine(FireStreamRoutine());
                    break;
                case FireMode.Shotgun:
                    StartCoroutine(FireShotgunRoutine());
                    break;
            }
        }

        void Update()
        {
            switch (moveMode)
            {
                case MoveMode.Linear:
                    MoveLinear(moveAngle);
                    break;
                case MoveMode.Circle:
                    MoveCircle();
                    break;
            }
        }

        // --- 이동 메서드 ---

        private void MoveLinear(float angle)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector2 dir = MathUtility.DegreeToVector2(angle);
                transform.Translate(dir * (moveSpeed * Time.deltaTime));
            }
        }

        private void MoveCircle()
        {
            Vector2 dir = MathUtility.DegreeToVector2(_currentOrbitAngle);
            transform.Translate(dir * (moveSpeed * Time.deltaTime));

            _currentOrbitAngle += circleSpeed * Time.deltaTime;
            if (_currentOrbitAngle >= 360f) _currentOrbitAngle -= 360f;
        }

        // --- 발사 루틴 ---

        private IEnumerator FireStreamRoutine()
        {
            int angle = startAngle;
            while (true)
            {
                SpawnBullet(angle);
                yield return new WaitForSeconds(streamFireRate);

                angle += angleInterval;
                if (angle > endAngle) angle = startAngle;
            }
        }

        private IEnumerator FireShotgunRoutine()
        {
            while (true)
            {
                for (int angle = startAngle; angle <= endAngle; angle += angleInterval)
                {
                    SpawnBullet(angle);
                }
                yield return new WaitForSeconds(shotgunCooldown);
            }
        }

        private void SpawnBullet(float angle)
        {
            if (bulletPrefab == null) return;

            Vector2 direction = MathUtility.DegreeToVector2(angle);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletContainer);
            bullet.transform.right = direction;
        }
    }
}