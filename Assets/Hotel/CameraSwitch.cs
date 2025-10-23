using UnityEngine;

// 새 입력 시스템을 쓸 때만 네임스페이스를 임포트
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

namespace MyFps
{
    public class CameraSwitch : MonoBehaviour
    {
        [Header("References")]
        public Transform player;       // (옵션) 필요 없으면 비워도 됨
        public Transform cameraRoot;   // 카메라의 부모(카메라 위치를 이동시킬 대상)

        [Header("Camera Offsets (local space)")]
        public Vector3 firstPersonOffset = new Vector3(0f, 1.6f, 0f);   // 머리 위치
        public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -4f);    // 어깨 뒤 위치

        [Header("Settings")]
        public float switchSmooth = 6f;   // 위치 전환 부드럽게
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

            // 부드럽게 보간
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * switchSmooth);

            // 카메라 위치 갱신 (local 기준)
            if (cameraRoot != null)
                cameraRoot.localPosition = currentOffset;
        }

        // ─────────────────────────────────────────────────────────────
        // 입력 호환 래퍼: 새/구 입력 시스템 모두 지원
        // ─────────────────────────────────────────────────────────────
        private bool TogglePressed()
        {
            // 새 입력 시스템만 켜져 있는 경우
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame;

            // 구 입력 시스템(또는 Both)인 경우
#else
            return Input.GetKeyDown(KeyCode.C);
#endif
        }
    }
}