using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICheckboxInput : MonoBehaviour
{
    [SerializeField]
    private SpringObject weight;
    [SerializeField]
    private bool defaultValue;
    [SerializeField]
    private string propertyName;
    private Toggle checkbox;

    void Start()
    {
        checkbox = transform.Find("Toggle").GetComponent<Toggle>();
        checkbox.onValueChanged.AddListener(checkbox_Change);
        checkbox.isOn = defaultValue;
    }

    void checkbox_Change(bool value)
    {
        weight.setPropertyValue(propertyName, value);
    }
}
