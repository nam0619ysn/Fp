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
    [SerializeField] float jumpHeight = 0f;  // ���� �ʿ� ������ 0

    [Header("Click-to-Move")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask groundMask = ~0;   // Ground ���̾ �����صθ� ���ƿ�
    [SerializeField] float navStoppingDistance = 0.1f;

    CharacterController cc;
    NavMeshAgent agent;
    Vector3 velocity;              // �߷� ó����
    bool keyboardMode;             // true�� Ű���� �̵� ���

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;          // ȸ���� ���� ����
        agent.stoppingDistance = navStoppingDistance;
        SetKeyboardMode(true);                 // �⺻: Ű���� ���
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        HandleKeyboard();
        HandleClickToMove();
        UpdateAnimationLike(); // (�ʿ� �� �ִϸ����� �Ķ���� ������Ʈ)
    }

    void HandleKeyboard()
    {
        // �Է� ��(�⺻ Input Manager). �� Input System ���� ���⸸ �ٲ� ����� ��.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v);
        bool hasInput = input.sqrMagnitude > 0.01f;

        // �Է��� ������ Ű���� ���� ��ȯ
        if (hasInput && !keyboardMode) SetKeyboardMode(true);

        if (!keyboardMode) return;

        // ī�޶� ���� �����¿�� �̵��ϰ� �ʹٸ�:
        Vector3 camForward = cam ? Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized : Vector3.forward;
        Vector3 camRight = cam ? cam.transform.right : Vector3.right;
        Vector3 move = (camForward * v + camRight * h).normalized * moveSpeed;

        // ȸ��(�̵� ���� �ٶ󺸱�)
        if (hasInput)
        {
            var look = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationLerp * Time.deltaTime);
        }

        // �߷�
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

        // CharacterController �̵�
        cc.Move((move + new Vector3(0, velocity.y, 0)) * Time.deltaTime);
    }

    void HandleClickToMove()
    {
        // ��Ŭ��: ������ ���� �� ������Ʈ ���� ��ȯ
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

        // ��Ŭ��: ��� �̵� ���
        if (Input.GetMouseButtonDown(1) && agent.enabled)
        {
            agent.ResetPath();
            SetKeyboardMode(true);
        }

        // ������Ʈ �̵� �� ȸ�� ����
        if (agent.enabled && agent.hasPath && agent.velocity.sqrMagnitude > 0.001f)
        {
            var dir = agent.velocity; dir.y = 0f;
            var look = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationLerp * Time.deltaTime);
        }

        // ������ ���� üũ �� �����ϸ� Ű���� ��� ���ư���(����)
        if (agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            SetKeyboardMode(true);
        }
    }

    void SetKeyboardMode(bool on)
    {
        keyboardMode = on;
        // ��� ��ȯ �� ������Ʈ Ȱ��/��Ȱ���� �浹 ����
        agent.enabled = !on;
        cc.enabled = on;
        if (on) velocity.y = -1f; // CC ��Ȱ���� ���� ���·� �ʱ�ȭ
    }

    // ����: �ִϸ��̼� �Ķ����(��: "Speed") ����
    void UpdateAnimationLike()
    {
        // ����: Animator�� �ִٸ� �ӵ� ����
        // var anim = GetComponent<Animator>();
        // if (!anim) return;
        // float speedParam = keyboardMode ? cc.velocity.magnitude : agent.velocity.magnitude;
        // anim.SetFloat("Speed", speedParam);
    }
}
