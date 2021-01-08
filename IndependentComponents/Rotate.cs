
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotatePerFrameX;
    [SerializeField] private float rotatePerFrameY;
    [SerializeField] private float rotatePerFrameZ; 

    private void Update()
    {
        transform.localEulerAngles += new Vector3(rotatePerFrameX, rotatePerFrameY, rotatePerFrameZ);
    }
}
