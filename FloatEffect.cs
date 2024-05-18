using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{

    public Vector2 amount;
    public float speed;
    private RectTransform rectTransform;
    private float seed;
    private Vector2 offset;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        offset = rectTransform.anchoredPosition;
        seed = Random.Range(-10000f, 1000f);
    }

    void Update()
    {
        float pos = seed + Time.unscaledTime * speed;
        Vector2 noise = new Vector2((Mathf.PerlinNoise(pos, pos) - 0.5f) * amount.x, (Mathf.PerlinNoise(pos - 1000, pos - 100) - 0.5f) * amount.y);
        rectTransform.anchoredPosition = offset + noise;
    }

}
