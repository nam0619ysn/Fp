using UnityEngine;

// �� �Է� �ý����� �� ���� ���ӽ����̽��� ����Ʈ
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

namespace MyFps
{
    public class CameraSwitch : MonoBehaviour
    {
        [Header("References")]
        public Transform player;       // (�ɼ�) �ʿ� ������ ����� ��
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
            if (cameraRoot == null) cameraRoot = transform;

            isFirstPerson = startAsFirstPerson;
            targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
            currentOffset = targetOffset;
            cameraRoot.localPosition = currentOffset;
        }

        void Update()
        {
            if (TogglePressed())
            {
                isFirstPerson = !isFirstPerson;
                targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
            }

            // �ε巴�� ����
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * switchSmooth);

            // ī�޶� ��ġ ���� (local ����)
            if (cameraRoot != null)
                cameraRoot.localPosition = currentOffset;
        }

        // ��������������������������������������������������������������������������������������������������������������������������
        // �Է� ȣȯ ����: ��/�� �Է� �ý��� ��� ����
        // ��������������������������������������������������������������������������������������������������������������������������
        private bool TogglePressed()
        {
            // �� �Է� �ý��۸� ���� �ִ� ���
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame;

            // �� �Է� �ý���(�Ǵ� Both)�� ���
#else
            return Input.GetKeyDown(KeyCode.C);
#endif
        }
    }
}