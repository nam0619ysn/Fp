// SimpleFollowCamera.cs
using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new(0, 10, -10);
    [SerializeField] float lerp = 8f;

    void LateUpdate()
    {
        if (!target) return;
        var desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * lerp);
        transform.LookAt(target.position, Vector3.up);
    }
}
