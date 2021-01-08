using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Animator animator;

    public bool HasReceivedShipment { get; set; }
    public bool CompletedShipmentPreviousDay { get; set; }

    public Date MinimalDateBeforeArrival;
    public TimeData TimeOfArrival; 

    [SerializeField] private PopUpElement popUpElement;
    [SerializeField] private AudioClip audioOnArrival;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        CompletedShipmentPreviousDay = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ActivateShip();
    }

    private void OnDisable()
    {
        DeactivateShip();
    }

    private void ActivateShip()
    {
        popUpElement.SetActive(false, true);
        CompletedShipmentPreviousDay = false;
        HasReceivedShipment = true;
        animator.SetTrigger("Start");
    }

    private void DeactivateShip()
    {
        popUpElement.SetActive(false, true);
        animator.SetTrigger("Stop");
    }

    public void OnArrived()
    {
        GameManager.Instance.AudioManager.PlayClip(audioOnArrival);
        popUpElement.SetActive(true);
    }
}
