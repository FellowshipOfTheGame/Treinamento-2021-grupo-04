using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovimentoPlataforma : MonoBehaviour
{
    public GameObject PauseScreenObject;

    private float yOffSet;

    public float freqY = 1f;
    public float ampY = 0.0001f;

    private float xOffSet;
    public float freqX = 1f;
    public float ampX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        yOffSet = UnityEngine.Random.value * 3.14f;
        xOffSet = UnityEngine.Random.value * 3.14f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseScreenObject.activeInHierarchy)
        {
            transform.Translate(new Vector2((float)Math.Sin(freqX * (Time.time + xOffSet)) * ampX, (float)Math.Sin(freqY * (Time.time + yOffSet)) * ampY));
        }
    }
}
