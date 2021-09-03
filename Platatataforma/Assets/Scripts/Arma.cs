using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour {
    public GameObject projetilPrefab;
    public GameObject arma;
    GameObject projetil;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnAtirar(){
        if(!projetil){
            projetil = Instantiate(projetilPrefab) as GameObject;
            Vector3 offset;
            offset.x = 1f;
            offset.y = 1f;
            offset.z = 0f;
            projetil.transform.position = arma.transform.position + offset;
        }
    }
}
