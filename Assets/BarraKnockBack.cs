using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraKnockBack : MonoBehaviour
{
    public Slider slider;

    public void SetValue(float Value){
        slider.value = Value;
    }
}
