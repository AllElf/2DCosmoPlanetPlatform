using UnityEngine;

public class RotateSphere : MonoBehaviour
{
    [SerializeField] Vector3 speed;
    void Update()
    { 
        transform.Rotate(speed);
    }
}
