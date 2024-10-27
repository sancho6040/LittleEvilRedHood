using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    private Vector3 _target;
    public void SetObjetivo(Vector3 target)
    {
        _target = target;
        // Puedes ajustar la velocidad del proyectil aquí si es necesario
        GetComponent<Rigidbody>().velocity = (target - transform.position).normalized * 10f;
    }

    void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.CompareTag("Player"))
        {
            Actor jugador = colision.gameObject.GetComponent<Actor>();
            if (jugador != null)
            {
                jugador.TakeDamage(10);  // Ajusta el valor de daño según sea necesario
            }
            Destroy(gameObject);
        }
    }

}
