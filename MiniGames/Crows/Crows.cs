using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crows : MonoBehaviour
{
    public bool OnlyActiveAfterTimeJump = false;
    [SerializeField] PopUpElement startButtonPopUpElement;

    private void OnEnable()
    {
        startButtonPopUpElement?.SetActive(false, true);
        startButtonPopUpElement?.SetActive(true);
    }
}
