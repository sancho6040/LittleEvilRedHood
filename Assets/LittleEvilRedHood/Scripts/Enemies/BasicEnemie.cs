using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public GameObject AlertImage;

    private EnemieState _state;
    private NavMeshAgent _agent;
    private bool _hasDestination;
    private bool _isPlayerOnSight;

    [Header("Ennemie Attack")]
    public GameObject ProyectilePrefab;
    public Transform ShootPosition;
    public float AttackDelay;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        SetPatrolDestination();
    }

    private void LateUpdate()
    {
        if (_state == EnemieState.Patrolling)
        {
            if (_agent?.remainingDistance <= 0.1 && !_hasDestination)
            {
                Invoke("SetPatrolDestination", 1f);
                _hasDestination = true; //evita llamar varias veces esta logica
            }
        }

    }

    public void SetPatrolDestination()
    {
        _state = EnemieState.Patrolling;
        AlertImage.SetActive(false);
        Vector3 puntoAleatorio = GenerarPosicionAleatoria();
        _agent?.SetDestination(puntoAleatorio);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaycastHit hit;
            Vector3 directionToPlayer = other.transform.position - transform.position;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit))
            {
                //revisa que no haya nada bloqueando la vista del enemigo al jugador
                if (hit.collider.CompareTag("Player"))
                {
                    _isPlayerOnSight = true;
                    transform.LookAt(other.transform);
                    StartCoroutine(DispararProyectil(other.transform.position));
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isPlayerOnSight = false;

        Invoke("SetPatrolDestination", 1f);
    }

    private IEnumerator DispararProyectil(Vector3 objetivo)
    {
        _agent.SetDestination(transform.position);
        _state = EnemieState.Attacking;
        //play vfx and sounds

        AlertImage.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        while (_isPlayerOnSight)
        {
            GameObject proyectil = Instantiate(ProyectilePrefab, ShootPosition.position, Quaternion.identity);
            proyectil.GetComponent<Proyectile>().SetObjetivo(objetivo);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
