using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour {
    [SerializeField] GameObject projetilPrefab;
    [SerializeField] Transform shotPosition;
    GameObject projetil;

    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Atirar(){
        if(!projetil){
            projetil = Instantiate(projetilPrefab, shotPosition.position, transform.rotation) as GameObject;
            projetil.GetComponent<Projetil>().SetOwner(playerController.GetPlayer());
        }
    }
}
