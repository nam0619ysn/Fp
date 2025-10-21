using UnityEngine;

using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 카메라(시선) 기준으로 정면 Raycast 하여 타겟까지 거리 측정.
    /// - 1/3인칭 전환(CameraSwitch)과 공존하도록 카메라 Transform을 명시적으로 사용
    /// - Player 레이어/자기 자신 무시를 위해 LayerMask 사용
    /// </summary>
    public class PlayerCasting : MonoBehaviour
    {
        [Header("Ray Settings")]
        public Transform rayOrigin;         // 보통 Main Camera Transform 지정
        public float maxDistance = 3.0f;    // 상호작용 거리 등 목적에 맞게
        public LayerMask hitMask = ~0;      // 맞출 레이어(기본: 전부). Player 레이어는 제외 권장.

        [Header("Debug (Read Only)")]
        public static float distanceFromTarget;
        public float toTarget;

        void Awake()
        {
            // 지정 안 했을 때 자동으로 메인 카메라 사용
            if (rayOrigin == null && Camera.main != null)
                rayOrigin = Camera.main.transform;

            distanceFromTarget = Mathf.Infinity;
            toTarget = distanceFromTarget;
        }

        // 카메라/플레이어 이동/회전이 끝난 뒤에 쏘는 게 안정적
        void LateUpdate()
        {
            if (rayOrigin == null)
            {
                distanceFromTarget = Mathf.Infinity;
                toTarget = distanceFromTarget;
                return;
            }

            RaycastHit hit;
            var origin = rayOrigin.position;
            var dir = rayOrigin.forward;

            // Player 자신을 피하고 싶다면: Player 레이어를 빼서 설정해두기
            // 예) hitMask = ~LayerMask.GetMask("Player");
            if (Physics.Raycast(origin, dir, out hit, maxDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                distanceFromTarget = hit.distance;
            }
            else
            {
                distanceFromTarget = Mathf.Infinity;
            }

            toTarget = distanceFromTarget;
        }

        void OnDrawGizmosSelected()
        {
            if (rayOrigin == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * Mathf.Min(maxDistance, 100f));
        }
    }
}