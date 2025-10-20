using UnityEngine;

namespace MyFps
{
    public class CameraSwitch : MonoBehaviour
    {
        [Header("References")]
        public Transform player;       // 플레이어 Transform
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
            if (cameraRoot == null)
                cameraRoot = transform;

            isFirstPerson = startAsFirstPerson;
            targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
            currentOffset = targetOffset;
        }

        void Update()
        {
            // C키로 시점 전환
            if (Input.GetKeyDown(KeyCode.C))
            {
                isFirstPerson = !isFirstPerson;
                targetOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
            }

            // 부드럽게 보간
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * switchSmooth);

            // 카메라 위치 갱신 (local 기준)
            cameraRoot.localPosition = currentOffset;
        }
    }
}
