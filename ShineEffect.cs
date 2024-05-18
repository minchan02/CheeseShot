using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShineEffect : MonoBehaviour
{

    private float seed;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        seed = Random.Range(-1f, 0f);
    }

    void Update()
    {
        image.material.SetFloat("_Progress", seed + Time.unscaledTime);
    }

}
