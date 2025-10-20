using UnityEngine;

namespace MyFps
{
    public class CameraSwitch : MonoBehaviour
    {
        [Header("References")]
        public Transform player;       // �÷��̾� Transform
        public Transform cameraRoot;   // ī�޶��� �θ�(ī�޶� ��ġ�� �̵���ų ���)

        [Header("Camera Offsets (local space)")]
        public Vector3 firstPersonOffset = new Vector3(0f, 1.6f, 0f);   // �Ӹ� ��ġ
        public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -4f);    // ��� �� ��ġ

        [Header("Settings")]
        public float switchSmooth = 6f;   // ��ġ ��ȯ �ε巴��
        public bool startAsFirstPerson = true;

        private bool isFirstPerson;
        private Vector3 currentOffset;
        private Vector3 targetOffset;

        void Start()
        {
            if (cameraRoot == null)
                cameraRoot = transform;

            isFirstPerson = startAsFirstPerson;
            targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
            currentOffset = targetOffset;
        }

        void Update()
        {
            // CŰ�� ���� ��ȯ
            if (Input.GetKeyDown(KeyCode.C))
            {
                isFirstPerson = !isFirstPerson;
                targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
            }

            // �ε巴�� ����
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * switchSmooth);

            // ī�޶� ��ġ ���� (local ����)
            cameraRoot.localPosition = currentOffset;
        }
    }
}
