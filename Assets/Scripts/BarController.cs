using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    
    public void SetMaxValue(int x)
    {
        
        slider.maxValue = x;
        slider.value = x;
       
    }
    public void SetMinValue(int x)
    {
        slider.minValue = x;
        slider.value = x;
    }
    public void SetValue(int y)
    {
        slider.value = y;
    }
}
