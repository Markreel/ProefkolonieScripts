using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStructure : MonoBehaviour, IFocusable
{
    [Header("Focus Settings: ")]
    [SerializeField] private float zoomAmountOnFocus = 1;
    public float ZoomAmountOnFocus { get { return zoomAmountOnFocus; } set { zoomAmountOnFocus = value; } }

    [Header("Popup Element: ")]
    [SerializeField] private PopUpElement popUpElement;

    public void OnFocus()
    {
        popUpElement?.SetActive(true);
    }

    public void OnUnfocus()
    {
        popUpElement?.SetActive(false);
    }
}
