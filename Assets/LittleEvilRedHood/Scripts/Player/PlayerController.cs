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

    [Header("Attack")]
    [SerializeField] float _attackSpeed = 1.5f;
    [SerializeField] float _attackDelay = 0.3f;
    [SerializeField] float _attackDistance = 1.5f;
    [SerializeField] int _attackDamage = 1;
    [SerializeField] ParticleSystem hitEffect;


    private bool _isPlayerBusy = false;
    private Interactable _target;

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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, _clickLayer))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                _target = hit.transform.GetComponent<Interactable>();
                if (_clickEffect != null)
                {
                    Instantiate(_clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), _clickEffect.transform.rotation);
                }
            }
            else
            {
                _target = null;
                _agent.destination = hit.point;
                if (_clickEffect != null)
                {
                    Instantiate(_clickEffect, hit.point += new Vector3(0f, 0.1f, 0f), _clickEffect.transform.rotation);
                }

            }
        }
    }

    private void Update()
    {
        FollowTarget();
        FaceTarget();
        SetAnimations();
    }

    private void FollowTarget()
    {
        if (_target == null) return;

        if (Vector3.Distance(_target.transform.position, transform.position) <= _attackDistance)
        {
            ReachDistance();
        }
        else
        {
            _agent.SetDestination(_target.transform.position);
        }
    }

    private void ReachDistance()
    {
        _agent.SetDestination(transform.position);

        if (_isPlayerBusy) return;

        _isPlayerBusy = true;

        switch (_target.InteractableType)
        {
            case InteractableType.Enemy:
                _animator.SetTrigger("Attack");
                Invoke(nameof(SendAttack), _attackDelay);
                Invoke(nameof(ResetBusyState), _attackSpeed);
                break;

            case InteractableType.Item:
                //animator.play("PickUp");
                _target.InteractWithItem();
                _target = null;
                Invoke(nameof(ResetBusyState), 0.5f);
                break;
        }
    }

    private void SendAttack()
    {
        if (_target == null) return;

        if(_target.thisActor._currentHealth <= 0)
        {
            _target = null;
            return;
        }

        Instantiate(hitEffect, _target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        _target.GetComponent<Actor>().TakeDamage(_attackDamage);
    }

    private void ResetBusyState()
    {
        _isPlayerBusy = false;
        SetAnimations();
    }

    private void FaceTarget()
    {
        if (_agent.destination == transform.position) return;

        Vector3 facing = Vector3.zero;
        if(_target != null)
        {
            facing = _target.transform.position;
        }
        else
        {
            facing = _agent.destination;
        }


        Vector3 direction = (facing - transform.position).normalized;
        if (direction.x == 0 && direction.z == 0) return;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _lookRotationSpeed);
    }

    private void SetAnimations()
    {
        if (_isPlayerBusy) return;

        if (_animator != null && _animator != null)
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
