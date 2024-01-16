using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISliderInput : MonoBehaviour
{
    [SerializeField]
    private SpringObject weight;
    [SerializeField]
    private float sliderMin;
    [SerializeField]
    private float sliderMax;
    [SerializeField]
    private float defaultValue;
    [SerializeField]
    private string propertyName;
    [SerializeField]
    private string labelText;
    private Slider slider;
    private TMP_InputField input;
    private TMP_Text label;


    void Start()
    {
        slider = transform.Find("Slider").GetComponent<Slider>();
        input = transform.Find("Input").GetComponent<TMP_InputField>();
        label = transform.Find("Label").GetComponent<TMP_Text>();

        slider.onValueChanged.AddListener(slider_Change);
        input.onValueChanged.AddListener(input_Change);

        slider.minValue = sliderMin;
        slider.maxValue = sliderMax;
        slider.value = defaultValue;

        input.text = defaultValue.ToString();
        label.text = labelText;
    }

    void slider_Change(float value)
    {
        weight.setPropertyValue(propertyName, value);
        input.text = value.ToString();
    }

    void input_Change(string value)
    {
        try
        {
            weight.setPropertyValue(propertyName, float.Parse(value));
            slider.value = float.Parse(value);
        }
        catch { }
    }

}
