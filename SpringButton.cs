using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SpringButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    [Header("Events")]
    public UnityEvent onClick;

    [Header("Settings")]
    public AnimationCurve inCurve;
    public AnimationCurve outCurve;
    public float speed = 4f;

    private bool selected;
    private float clickTime;

    void Start()
    {
        
    }

    void Update()
    {
        clickTime = Mathf.MoveTowards(clickTime, 1, Time.unscaledDeltaTime * speed);
        transform.localScale = Vector3.one * (selected ? inCurve.Evaluate(clickTime) : outCurve.Evaluate(clickTime));
    }

    public void OnPointerDown(PointerEventData PED)
    {
        selected = true;
        clickTime = 0;
    }

    public void OnPointerUp(PointerEventData PED)
    {
        selected = false;
        clickTime = 0;
    }

    public void OnPointerClick(PointerEventData PED)
    {
        onClick.Invoke();
    }

}
