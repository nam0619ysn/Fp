using UnityEngine;

using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// ī�޶�(�ü�) �������� ���� Raycast �Ͽ� Ÿ�ٱ��� �Ÿ� ����.
    /// - 1/3��Ī ��ȯ(CameraSwitch)�� �����ϵ��� ī�޶� Transform�� ��������� ���
    /// - Player ���̾�/�ڱ� �ڽ� ���ø� ���� LayerMask ���
    /// </summary>
    public class PlayerCasting : MonoBehaviour
    {
        [Header("Ray Settings")]
        public Transform rayOrigin;         // ���� Main Camera Transform ����
        public float maxDistance = 3.0f;    // ��ȣ�ۿ� �Ÿ� �� ������ �°�
        public LayerMask hitMask = ~0;      // ���� ���̾�(�⺻: ����). Player ���̾�� ���� ����.

        [Header("Debug (Read Only)")]
        public static float distanceFromTarget;
        public float toTarget;

        void Awake()
        {
            // ���� �� ���� �� �ڵ����� ���� ī�޶� ���
            if (rayOrigin == null && Camera.main != null)
                rayOrigin = Camera.main.transform;

            distanceFromTarget = Mathf.Infinity;
            toTarget = distanceFromTarget;
        }

        // ī�޶�/�÷��̾� �̵�/ȸ���� ���� �ڿ� ��� �� ������
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

            // Player �ڽ��� ���ϰ� �ʹٸ�: Player ���̾ ���� �����صα�
            // ��) hitMask = ~LayerMask.GetMask("Player");
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