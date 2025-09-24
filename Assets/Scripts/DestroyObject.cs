using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] float _seconds;
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_seconds);
        Destroy(gameObject);
    }
}
