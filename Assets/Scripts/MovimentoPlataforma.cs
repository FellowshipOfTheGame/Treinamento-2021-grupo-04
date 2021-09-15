using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovimentoPlataforma : MonoBehaviour
{
    //public GameObject PauseScreenObject;
    //public GameObject EndScreenObject;

    private float yOffSet;
    public float freqY = 1f;
    public float ampY = 0.0001f;

    private float xOffSet;
    public float freqX = 1f;
    public float ampX = 0f;

    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        xOffSet = UnityEngine.Random.value * Mathf.PI + Mathf.PI / 2; // somando com pi/2 para que o seno varie de 1 a -1 com este offset inicial
        yOffSet = UnityEngine.Random.value * Mathf.PI + Mathf.PI / 2;

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!PauseScreenObject.activeInHierarchy && !EndScreenObject.activeInHierarchy)
        {
            transform.Translate(new Vector2((float)Math.Sin(freqX * (Time.time + xOffSet)) * ampX, (float)Math.Sin(freqY * (Time.time + yOffSet)) * ampY));
        }*/

        Vector2 newPosition = new Vector2(Mathf.Sin(freqX * (Time.time + xOffSet)) * ampX, Mathf.Sin(freqY * (Time.time + yOffSet)) * ampY);
        newPosition += startPos; // sempre relativo à posição inicial

        // Translate recebe vetor direção que é a posição q desejamos chegar menos a que estamos
        transform.Translate(newPosition - (Vector2) transform.position); 
    }
}
