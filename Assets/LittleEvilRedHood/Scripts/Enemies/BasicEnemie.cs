using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemieState
{
    Idle,
    Patrolling,
    Running,
    Attacking,
}

public class BasicEnemie : MonoBehaviour
{
    public float PatrolRange = 10f;
    public LayerMask FloorLayer;

    private NavMeshAgent _agent;
    private bool _hasDestination;

    [Header("Ennemie Attack")]
    public GameObject ProyectilePrefab;
    public Transform ShootPosition;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        SetPatrolDestination();
    }

    private void LateUpdate()
    {
        if(_agent?.remainingDistance <= 0.1 && !_hasDestination)
        {
            Invoke("SetPatrolDestination", 1f);
            _hasDestination = true; //evita llamar varias veces esta logica
        }
    }

    public void SetPatrolDestination()
    {
        Vector3 puntoAleatorio = GenerarPosicionAleatoria();
        if (puntoAleatorio != Vector3.zero)
        {
            _agent?.SetDestination(puntoAleatorio);
        }
        _hasDestination = false;
    }

    private Vector3 GenerarPosicionAleatoria()
    {
        Vector3 puntoAleatorio = Vector3.zero;
        bool encontrado = false;
        while (!encontrado)
        {
            Vector3 posicionAleatoria = new Vector3(Random.Range(-PatrolRange, PatrolRange), 0f, Random.Range(-PatrolRange, PatrolRange)) + transform.position;
            RaycastHit hit;
            if (Physics.Raycast(posicionAleatoria + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity, FloorLayer))
            {
                puntoAleatorio = hit.point;
                encontrado = true;
            }
        }
        return puntoAleatorio;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DispararProyectil(other.transform.position);
        }
    }

    void DispararProyectil(Vector3 objetivo)
    {
        GameObject proyectil = Instantiate(ProyectilePrefab, ShootPosition.position, Quaternion.identity);
        proyectil.GetComponent<Proyectile>().SetObjetivo(objetivo);
    }
}
