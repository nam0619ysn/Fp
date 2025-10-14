// PlayerHybridMover.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerHybridMover : MonoBehaviour
{
    [Header("Keyboard Move")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationLerp = 12f;
    [SerializeField] float gravity = -20f;
    [SerializeField] float jumpHeight = 0f;  // 점프 필요 없으면 0

    [Header("Click-to-Move")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask groundMask = ~0;   // Ground 레이어를 지정해두면 좋아요
    [SerializeField] float navStoppingDistance = 0.1f;

    CharacterController cc;
    NavMeshAgent agent;
    Vector3 velocity;              // 중력 처리용
    bool keyboardMode;             // true면 키보드 이동 모드

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;          // 회전은 직접 제어
        agent.stoppingDistance = navStoppingDistance;
        SetKeyboardMode(true);                 // 기본: 키보드 모드
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        HandleKeyboard();
        HandleClickToMove();
        UpdateAnimationLike(); // (필요 시 애니메이터 파라미터 업데이트)
    }

    void HandleKeyboard()
    {
        // 입력 축(기본 Input Manager). 새 Input System 쓰면 여기만 바꿔 끼우면 됨.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v);
        bool hasInput = input.sqrMagnitude > 0.01f;

        // 입력이 들어오면 키보드 모드로 전환
        if (hasInput && !keyboardMode) SetKeyboardMode(true);

        if (!keyboardMode) return;

        // 카메라 기준 전후좌우로 이동하고 싶다면:
        Vector3 camForward = cam ? Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized : Vector3.forward;
        Vector3 camRight = cam ? cam.transform.right : Vector3.right;
        Vector3 move = (camForward * v + camRight * h).normalized * moveSpeed;

        // 회전(이동 방향 바라보기)
        if (hasInput)
        {
            var look = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationLerp * Time.deltaTime);
        }

        // 중력
        if (cc.isGrounded)
        {
            velocity.y = -1f;
            if (jumpHeight > 0 && Input.GetKeyDown(KeyCode.Space))
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // CharacterController 이동
        cc.Move((move + new Vector3(0, velocity.y, 0)) * Time.deltaTime);
    }

    void HandleClickToMove()
    {
        // 좌클릭: 목적지 설정 → 에이전트 모드로 전환
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000f, groundMask))
            {
                if (NavMesh.SamplePosition(hit.point, out var navHit, 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                    SetKeyboardMode(false);
                }
            }
        }

        // 우클릭: 경로 이동 취소
        if (Input.GetMouseButtonDown(1) && agent.enabled)
        {
            agent.ResetPath();
            SetKeyboardMode(true);
        }

        // 에이전트 이동 중 회전 보정
        if (agent.enabled && agent.hasPath && agent.velocity.sqrMagnitude > 0.001f)
        {
            var dir = agent.velocity; dir.y = 0f;
            var look = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationLerp * Time.deltaTime);
        }

        // 목적지 도착 체크 → 도착하면 키보드 모드 돌아가기(선택)
        if (agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            SetKeyboardMode(true);
        }
    }

    void SetKeyboardMode(bool on)
    {
        keyboardMode = on;
        // 모드 전환 시 컴포넌트 활성/비활성로 충돌 방지
        agent.enabled = !on;
        cc.enabled = on;
        if (on) velocity.y = -1f; // CC 재활성시 착지 상태로 초기화
    }

    // 선택: 애니메이션 파라미터(예: "Speed") 갱신
    void UpdateAnimationLike()
    {
        // 예시: Animator가 있다면 속도 세팅
        // var anim = GetComponent<Animator>();
        // if (!anim) return;
        // float speedParam = keyboardMode ? cc.velocity.magnitude : agent.velocity.magnitude;
        // anim.SetFloat("Speed", speedParam);
    }
}
