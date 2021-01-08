using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MoneyGraphSystem : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Transform> months = new List<Transform>();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        AppendMonths();
    }

    private void Start()
    {
        //GenerateGraph();
    }

    private void AppendMonths()
    {
        foreach (Transform _child in transform)
        {
            months.Add(_child);
        }
    }

    public void GenerateGraph()
    {
        Vector3 _pos = months[GameManager.Instance.TimeSystem.CurrentMonth - 2].localPosition;
        _pos.z = (10f / GameManager.Instance.MoneyMaximum * GameManager.Instance.Money)-5f;


        months[GameManager.Instance.TimeSystem.CurrentMonth - 2].localPosition = _pos;

        lineRenderer.positionCount = GameManager.Instance.TimeSystem.CurrentMonth-1;
        for (int i = 0; i < GameManager.Instance.TimeSystem.CurrentMonth-1; i++)
        {
            lineRenderer.SetPosition(i, months[i].position);
        }
    }
}
