using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float speed = 400;
    public Rigidbody2D rb2d;
    public Vector2 vel = new Vector2(1, 1);
    float lifeTime = 2f;
    float dano = 0.2f;

    // knockback force
    [SerializeField] float knockbackAngle;
    [SerializeField] float knockbackForce;
    Vector2 knockbackVector;

    int owner;

    // Start is called before the first frame update,
    void Start()
    {
        vel.x *= Mathf.Sign(transform.right.x);

        rb2d.AddForce(vel * speed);

        // vetor resultante tem mesma dire��o que velocidade do proj�til, com �ngulo e intensidade pr�-especificados
        knockbackVector = DegreeToVector2(knockbackAngle) * knockbackForce;
        knockbackVector.x *= Mathf.Sign(transform.right.x);

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Player1") && owner != 1  || collider.CompareTag("Player2") && owner != 2)
        {
            collider.GetComponent<PlayerController>().Knockback(knockbackVector, dano);
            Destroy(this.gameObject);
        }
        else if (!collider.CompareTag("Player1") && !collider.CompareTag("Player2"))
        {
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0f;
            Destroy(this.gameObject, 0.5f);
        }
    }

    public void SetOwner(int player)
    {
        owner = player;
    }

    // Retorna o vetor normalizado corespondente ao �ngulo passado
    Vector2 DegreeToVector2(float degree)
    {
        degree = degree * Mathf.Deg2Rad;
        Vector2 vec = new Vector2(Mathf.Cos(degree), Mathf.Sin(degree)).normalized;

        return vec;
    }
}
