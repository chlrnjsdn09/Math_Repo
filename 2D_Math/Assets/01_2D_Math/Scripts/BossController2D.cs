using UnityEngine;
using System.Collections;

namespace Lesson2D
{
    public class BossController2D : MonoBehaviour
    {
        [Header("탄막 설정")]
        public GameObject bulletPrefab;      // 보스용 총알 프리팹 (플레이어와 동일하게 Bullet2D 사용 가능)
        public Transform bulletContainer;
        public float fireRate = 1.5f;        // 발사 간격 (초)
        public int bulletCount = 8;          // 방사형 발사 시 탄막 개수

        private Transform _playerTransform;

        void Start()
        {
            // 씬 내의 플레이어를 찾아 목표물로 설정
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }

            // 주기적인 탄막 발사 패턴 시작
            StartCoroutine(ShootPatternLoop());
        }

        private IEnumerator ShootPatternLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);

                // 가만히 있는 상태에서 패턴 실행
                ShootAtPlayer();
                ShootRadialBarrage();
            }
        }

        /// <summary>
        /// 보스 주변 360도 방향으로 균등하게 방사형 탄막을 발사하는 패턴
        /// </summary>
        private void ShootRadialBarrage()
        {
            if (bulletPrefab == null) return;

            float angleStep = 360f / bulletCount;
            float angle = 0f;

            for (int i = 0; i < bulletCount; i++)
            {
                // 발사 방향 계산
                float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0);
                Vector2 fireDirection = (bulletMoveVector - transform.position).normalized;

                // 총알 생성 및 설정
                GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletContainer);
                bulletObj.transform.right = fireDirection;

                Bullet2D bullet = bulletObj.GetComponent<Bullet2D>();
                if (bullet != null)
                {
                    bullet.isEnemyBullet = true; // 보스가 쏜 총알로 판정
                }

                angle += angleStep;
            }
        }

        /// <summary>
        /// 플레이어의 현재 위치를 향해 조준 사격을 가합니다.
        /// </summary>
        private void ShootAtPlayer()
        {
            // 플레이어가 씬에 없거나 파괴된 상태, 또는 총알 프리팹이 없다면 실행하지 않음
            if (_playerTransform == null || bulletPrefab == null) return;

            // 보스 위치에서 플레이어 위치를 향하는 방향 벡터를 계산하고 정규화(normalized)합니다.
            Vector2 fireDirection = ((Vector2)_playerTransform.position - (Vector2)transform.position).normalized;

            // 보스의 현재 위치에 총알을 생성합니다.
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletContainer);

            // 총알의 진행 방향(transform.right)을 플레이어가 위치한 방향 벡터로 회전시킵니다 [1].
            bulletObj.transform.right = fireDirection;

            // 플레이어가 맞을 수 있도록 보스의 탄막(isEnemyBullet = true)으로 설정합니다.
            Bullet2D bullet = bulletObj.GetComponent<Bullet2D>();
            if (bullet != null)
            {
                bullet.isEnemyBullet = true;
            }
        }
    }
}