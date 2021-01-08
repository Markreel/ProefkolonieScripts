using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool isActive = true;

    public void SetActive(bool _value)
    {
        isActive = _value;
    }

    private void Update()
    {
        if (isActive) { transform.LookAt(Camera.main.transform); }
    }
}
