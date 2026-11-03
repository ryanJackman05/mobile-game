using System;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] bool withConstraints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // assume correct layer interaction exclusions - > check project config and gameobject layer
        SendMessageUpwards("Crash", withConstraints);
    }
}
