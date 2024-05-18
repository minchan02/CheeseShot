using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraFallow : MonoBehaviour
{

    [Header("OT")]
    public new Camera camera;
    public Transform target;
    public Joystick joystick;

    [Header("Settings")]
    public Vector3 offset;
    public float focusSpeed; //Lerp와 약간 다른 사용 방법, 값이 낮을수록 더 빠름
    public float focusAmount;
    public float fovSpeed;
    public float startFOV;
    public float defaultFOV;
    public float maxFOV;

    [Header("Debug")]
    [ReadOnly] public Vector2 focus;
    [ReadOnly] public Vector2 focusVel;
    [ReadOnly] public float fov;

    void Start()
    {
        camera.orthographicSize = startFOV;
    }

    void LateUpdate()
    {
        focus = Vector2.SmoothDamp(focus, joystick.GetControl() * focusAmount, ref focusVel, focusSpeed);
        transform.position = new Vector3(target.position.x, target.position.y, 0) + offset + (Vector3)focus;
        fov = (joystick.GetControlRaw() == Vector2.zero ? maxFOV : defaultFOV) * Mathf.Sqrt(0.9f + Mathf.Clamp(Player.instance.TotalCount, 10, 25) * 0.1f);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, fov, Time.deltaTime * fovSpeed);
    }
}
