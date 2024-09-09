using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointObject : MonoBehaviour
{
    [SerializeField] private int value;
    private int previousValue;
    [SerializeField] [Range(0f, 1f)] private float fillAmount = 1;
    private float previousFillAmount;

    [Header("In Prefab Objects")]
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image firstImage;

    private void Start() { setValue(value); }

    // Point value
    public void setValue(int value) { this.value = value; showValue(); }
    public int getValue() { return value; }
    private void showValue() { valueText.text = value.ToString(); }

    // Fill amount
    public void setFillAmount(float fillAmount) { this.fillAmount = fillAmount; showFillAmount(); }
    public float getFillAmount() { return fillAmount; }
    public void showFillAmount() { firstImage.fillAmount = fillAmount; }


    private void OnValidate()
    {
        if (value != previousValue) { previousValue = value; showValue(); }
        if (fillAmount != previousFillAmount) { firstImage.fillAmount = fillAmount; showFillAmount();  }
    }

}
