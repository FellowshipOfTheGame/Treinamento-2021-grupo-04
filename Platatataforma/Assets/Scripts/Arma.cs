using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour {
    public GameObject proj_guia;
    private GameObject proj;
    public GameObject arma;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnAtirar(){
        proj = Instantiate(proj_guia);
        proj.transform.position = arma.transform.position;
        Destroy (proj, 1.5f);
    }
}
