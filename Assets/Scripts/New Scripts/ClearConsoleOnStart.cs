#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

public class ClearConsoleOnStart : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(StartClear());
    }

    IEnumerator StartClear()
    {
        yield return new WaitForSeconds(0.0f);

        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod?.Invoke(null, null);
    }
}
#endif