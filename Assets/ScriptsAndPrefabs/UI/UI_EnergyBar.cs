using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnergyBar : MonoBehaviour
{
    [Range(0,1)]
    public float value;
    private RectTransform fillArea;
    private RectTransform fill;

    private void Start()
    {
        fillArea = transform.Find("Fill Area").GetComponent<RectTransform>();
        fill = fillArea.Find("Fill").GetComponent<RectTransform>();
    }

    private void Update()
    {
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillArea.rect.width * value);
    }
}
