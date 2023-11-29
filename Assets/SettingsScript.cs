using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SettingsScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI sliderText = null;

    [SerializeField] private float maxSliderAmount = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        sliderText.text = localValue.ToString("0");
    }
}
