using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    Rigidbody2D rb2d;
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Player1") || collider.CompareTag("Player2")){
            collider.GetComponent<PlayerController>().Reload();
            Destroy(this.gameObject);
        }
        else{
            Destroy(this.gameObject, 15f);
        }
    }
}
