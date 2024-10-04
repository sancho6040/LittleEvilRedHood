using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InteractableType
{
    Enemy,
    Item
}

public class Interactable : MonoBehaviour
{
    public Actor thisActor { get; private set; }
    public InteractableType InteractableType { get; private set; }

    private void Awake()
    {
        if(InteractableType == InteractableType.Enemy)
        {
            thisActor = GetComponent<Actor>();
        }
    }

    public void InteractWithItem()
    {
        Destroy(gameObject);
    }
}
