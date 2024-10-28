using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        // Hacer que el objeto mire a la cámara
        transform.LookAt(Camera.main.transform);
    }
}
