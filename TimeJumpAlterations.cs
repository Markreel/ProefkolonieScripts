using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeJumpAlterations : MonoBehaviour
{
    [SerializeField] private List<GameObject> disableOnAlter;
    [SerializeField] private List<GameObject> enableOnAlter;

    [SerializeField] private UnityEvent onAlterEvent;

    public void Alter()
    {
        //Disable corresponding objects
        foreach (var _obj in disableOnAlter)
        {
            _obj.SetActive(false);
        }

        //Enable corresponding objects
        foreach (var _obj in enableOnAlter)
        {
            _obj.SetActive(true);
        }

        //Invoke corresponding events
        onAlterEvent.Invoke();
    }
}
