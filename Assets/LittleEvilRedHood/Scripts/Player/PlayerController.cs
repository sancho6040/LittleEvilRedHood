using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private CustomActions _input;
    private NavMeshAgent _agent;
    private Animator _animator;

    [Header("Movemnt")]
    [SerializeField] private ParticleSystem _clickEffect;
    [SerializeField] private LayerMask _clickLayer;

    private float _lookRotationSpeed = 8f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _input = new CustomActions();
        AssignInput();
    }

    private void AssignInput()
    {
        _input.Main.Move.performed += ctx => ClickToMove();
    }

    private void ClickToMove()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, _clickLayer))
        {
            _agent.destination = hit.point;
            if(_clickEffect != null)
            {
                Instantiate(_clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), _clickEffect.transform.rotation);
            }
        }
    }

    private void Update()
    {
        FaceTarget();
        SetAnimations();
    }

    private void FaceTarget()
    {
        Vector3 direction = (_agent.destination - transform.position).normalized;
        if (direction.x == 0 && direction.z == 0) return;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _lookRotationSpeed);
    }

    private void SetAnimations()
    {
        if(_animator != null && _animator != null)
        {
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
    }

    private void OnEnable()
    {
        _input?.Enable();
    }

    private void OnDisable()
    {
        _input?.Disable();
    }
}
