using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text ammoText;

    public void ChangeText(int value){
        ammoText.text = "Ammo: " + value + "/15";
    }
}
