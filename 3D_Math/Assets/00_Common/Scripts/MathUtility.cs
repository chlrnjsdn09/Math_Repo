using UnityEngine;

namespace EducationalMath
{
    public static class MathUtility
    {
        /// <summary>
        /// 2D 육십분법 각도(Degree)를 2D 벡터(XY)로 변환
        /// </summary>
        public static Vector2 DegreeToVector2(float degree)
        {
            float rad = degree * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        /// <summary>
        /// 3D 평면(XZ) 각도(Degree)를 3D 수평 방향 벡터(Vector3)로 변환
        /// </summary>
        public static Vector3 DegreeToVector3Flat(float degree)
        {
            float rad = degree * Mathf.Deg2Rad;
            // XZ 평면: X축이 가로(Cos), Z축이 세로(Sin), Y축은 0
            return new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
        }

        /// <summary>
        /// 3D 수평 방향 벡터(XZ)를 각도(Degree)로 변환
        /// </summary>
        public static float Vector3FlatToDegree(Vector3 direction)
        {
            return Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        }
    }
}