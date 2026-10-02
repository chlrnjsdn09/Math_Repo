using System.Collections;
using UnityEngine;
using EducationalMath;

namespace Lesson3D
{
    public enum ShotPattern
    {
        Stream,
        Shotgun
    }

    public class BulletSpawner3D : MonoBehaviour
    {
        [Header("총알 프리팹 및 컨테이너")]
        public GameObject bulletPrefab;
        public Transform bulletContainer;
        [Header("총알 발사 패턴 설정")]
        public ShotPattern pattern = ShotPattern.Stream;
        public float fireRate = 0.05f;
        public float shotgunCooldown = 3f;
        public int angleInterval = 10;
        public float startAngle = 30f;
        public float endAngle = 330f;
        public bool pingPong = true;

        void Start()
        {
            switch (pattern)
            {
                case ShotPattern.Stream:
                    StartCoroutine(FireStreamRoutine());
                    break;
                case ShotPattern.Shotgun:
                    StartCoroutine(FireShotgunRoutine());
                    break;
            }
        }

        private IEnumerator FireStreamRoutine()
        {
            float currentAngle = startAngle;
            float sign = 1f;

            while (true)
            {
                SpawnBullet(currentAngle);
                yield return new WaitForSeconds(fireRate);

                if (pingPong)
                {
                    currentAngle += angleInterval * sign;
                    if (currentAngle >= endAngle)
                    {
                        currentAngle = endAngle;
                        sign = -1f;
                    }
                    else if (currentAngle <= startAngle)
                    {
                        currentAngle = startAngle;
                        sign = 1f;
                    }
                }
                else
                {
                    currentAngle += angleInterval;
                    if (currentAngle > endAngle) currentAngle = startAngle;
                }
            }
        }

        private IEnumerator FireShotgunRoutine()
        {
            while (true)
            {
                for (float angle = startAngle; angle <= endAngle; angle += angleInterval)
                {
                    SpawnBullet(angle);
                }
                yield return new WaitForSeconds(shotgunCooldown);
            }
        }

        private void SpawnBullet(float angle)
        {
            if (bulletPrefab == null) return;

            Vector2 dir = MathUtility.DegreeToVector2(angle);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletContainer);
            bullet.transform.right = (Vector3)dir;
        }
    }
}