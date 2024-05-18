using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{

    public UIManager uimanager;
    
    [Header("OT")]
    public RectTransform joystick;
    public RectTransform joystickBG;
    public RectTransform canvasRect;
    public PlayerInput playerInput;

    [Header("Settings")]
    public float joystickRadius;
    public float joystickY;
    public float joystickSmooth;
    public Toggle toggle;

    [Header("Debug")]
    public bool fixing = true;
    private EventSystem eventSystem;
    private Vector2 joystickVec;
    private Vector2 directionVec;
    private Vector2 joystickPivot;
    private Vector2 joystickOffset;
    private Vector2 controlVecRaw;
    private Vector2 controlVec;
    private float screenMul;
    private float lastWidth;
    private int prevTouchCount;
    private bool pressed;
    public bool fix;

    void Start()
    {
        lastWidth = Screen.width;
        screenMul = Mathf.Sqrt(Screen.width * Screen.height / 2073600f);
        joystickPivot = new Vector2(Screen.width / 2f / screenMul, Screen.height * joystickY / screenMul);
        joystick.anchoredPosition = joystickPivot;
        joystickBG.anchoredPosition = joystickPivot;
        joystickOffset = joystickPivot;
        eventSystem = EventSystem.current;
        fix = true;
        uimanager.ChangeJoyButtonImage();
    }

    void Update()
    {
        Vector2 mousePos = GetMousePos();
        bool mouseDown = Input.GetMouseButtonDown(0);
        bool touchDown = Input.touchCount - prevTouchCount > 0;
        bool mousePressed = Input.GetMouseButton(0);
        bool touchPressed = Input.touchCount > 0;
        bool mouseUp = Input.GetMouseButtonUp(0);
        bool touchUp = Input.touchCount - prevTouchCount < 0;
        bool thisDown = mousePos.y <= Screen.height / screenMul - 256 && Time.timeScale != 0;
        if ((mouseDown || touchDown) && thisDown)
        {
            if (Screen.width != lastWidth)
            {
                lastWidth = Screen.width;
                screenMul = Mathf.Sqrt(Screen.width * Screen.height / 2073600f);
                joystickPivot = new Vector2(Screen.width / 2f / screenMul, Screen.height * joystickY / screenMul);
                joystick.anchoredPosition = joystickPivot;
                joystickBG.anchoredPosition = joystickPivot;
                joystickOffset = joystickPivot;
            }
            else
            {
                joystickPivot = new Vector2(Screen.width / 2f / screenMul, Screen.height * joystickY / screenMul);
            }
            if (fixing == false)
            {
                joystick.anchoredPosition = mousePos;
                joystickBG.anchoredPosition = mousePos;
                joystickOffset = mousePos;
            }
            else
            {
                joystick.anchoredPosition = joystickPivot;
                joystickOffset = joystickPivot;
            }
            pressed = true;
        }
        if ((mousePressed || touchPressed) && pressed)
        {
            joystickVec = (mousePos - joystickOffset).normalized;
            float joystickDist = Mathf.Clamp(Vector2.Distance(mousePos, joystickOffset), 0, joystickRadius);
            joystick.anchoredPosition = joystickOffset + joystickVec * joystickDist;
        }
        if ((mouseUp || touchUp) && pressed)
        {
            joystickVec = Vector2.zero;
            if (fixing)
            {
                joystickBG.anchoredPosition = joystickPivot;
            }
            joystick.anchoredPosition = joystickBG.anchoredPosition;
            pressed = false;
        }
        controlVecRaw = directionVec == Vector2.zero ? joystickVec : directionVec;
        controlVec = Vector2.Lerp(controlVec, controlVecRaw, Time.deltaTime * joystickSmooth);
        prevTouchCount = Input.touchCount;

        fixing = fix;
    }

    public Vector2 GetMousePos()
    {
        Vector2 touchPos = Input.touchCount > 0 ? Touchscreen.current.primaryTouch.position.ReadValue() : Mouse.current.position.ReadValue();
        return touchPos / screenMul;
    }

    public void OnDirection(InputAction.CallbackContext value)
    {
        directionVec = value.ReadValue<Vector2>().normalized;
    }

    public void OnChanged()
    {
        fix = !fix;
        fixing = fix;
    }

    public Vector2 GetControl()
    {
        return controlVec;
    }

    public Vector2 GetControlRaw()
    {
        return controlVecRaw;
    }

    public bool GetFix()
    {
        return fix;
    }

}
