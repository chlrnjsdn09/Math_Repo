using UnityEngine;

namespace Lesson2D
{
    public class AimShooter2D : MonoBehaviour
    {
        [Header("플레이어 이동 설정")]
        public float moveSpeed = 5f;

        [Header("총알 프리팹 및 컨테이너")]
        public GameObject bulletPrefab; // [2]
        public Transform bulletContainer; // [2]
        public GameObject guideLine; // [2]

        [Header("조준 및 반동 설정")]
        public float detectionRange = 4f;    // 가이드라인 표시 감지 사거리 [2]
        public float recoilDistance = 0.5f;   // 발사 시 반동 거리 [2]
        public float spawnOffset = 0.5f;      // 총구 스폰 오프셋 거리 [2]

        private Camera _mainCamera;
        private Rigidbody2D _rb2d; // 플레이어 물리 이동을 위한 Rigidbody2D

        void Start()
        {
            _mainCamera = Camera.main; // [2]
            _rb2d = GetComponent<Rigidbody2D>();

            // 플레이어가 회전하지 않도록 고정
            if (_rb2d != null)
            {
                _rb2d.freezeRotation = true;
                _rb2d.gravityScale = 0f; // 탑다운 2D이므로 중력 제거
            }
        }

        void Update()
        {
            // 1. WASD 이동 처리
            MovePlayer();

            // 2. 조준 및 발사 처리 [2]
            Vector2 mouseWorldPos = GetMouseWorldPosition(); // [2]
            Vector2 aimDirection = mouseWorldPos - (Vector2)transform.position; // [2]

            UpdateGuideLine(aimDirection); // [2]

            if (Input.GetMouseButtonDown(0)) // [2]
            {
                SpawnBullet(aimDirection.normalized); // [2]
            }
        }

        /// <summary>
        /// WASD 입력을 이용한 플레이어 4방향 이동
        /// </summary>
        private void MovePlayer()
        {
            float moveX = Input.GetAxisRaw("Horizontal"); // A, D / 좌, 우
            float moveY = Input.GetAxisRaw("Vertical");   // W, S / 상, 하
            Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

            if (_rb2d != null)
            {
                _rb2d.linearVelocity = moveDirection * moveSpeed;
            }
            else
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }

        /// <summary>
        /// 마우스 방향으로 총알 생성 및 플레이어 반동 처리
        /// </summary>
        private void SpawnBullet(Vector2 fireDirection)
        {
            if (bulletPrefab == null) return;

            Vector3 spawnPosition = transform.position + (Vector3)(fireDirection * spawnOffset); // [2]
            GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity, bulletContainer); // [2]
            bulletObj.transform.right = fireDirection; // [2]

            // 플레이어가 쏜 탄막임을 명시
            Bullet2D bullet = bulletObj.GetComponent<Bullet2D>();
            if (bullet != null)
            {
                bullet.isEnemyBullet = false;
            }

            // 발사 방향의 반대 방향으로 순간 반동 이동 [2]
            //transform.Translate(-fireDirection * recoilDistance);
        }

        private void UpdateGuideLine(Vector2 aimVector)
        {
            if (guideLine == null) return;

            bool isWithinRange = aimVector.magnitude < detectionRange; // [2]
            guideLine.SetActive(isWithinRange); // [2]

            if (isWithinRange && aimVector.sqrMagnitude > 0.001f) // [2]
            {
                guideLine.transform.right = aimVector.normalized; // [2]
            }
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector3 mouseScreenPos = Input.mousePosition; // [2]
            return _mainCamera.ScreenToWorldPoint(mouseScreenPos); // [2]
        }
    }
}
