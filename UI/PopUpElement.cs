using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpElement : MonoBehaviour
{
    [SerializeField] private Vector3 closedScale = Vector3.zero;
    [SerializeField] private Vector3 openedScale = Vector3.one;
    [SerializeField] private float lerpScaleDuration = 0.5f;
    [Space]
    [SerializeField] private bool activeOnAwake = false;
    [SerializeField] private bool lookAtCamera = true;
    private LookAtCamera lookAtCameraScript;

    private Coroutine setActiveRoutine;

    protected virtual void Awake()
    {
        transform.localScale = activeOnAwake ? openedScale : closedScale;
        if (lookAtCamera && lookAtCameraScript == null) { lookAtCameraScript = gameObject.AddComponent<LookAtCamera>(); }
    }

    public virtual void SetActive(bool _value, bool _instantaneously = false)
    {
        if (setActiveRoutine != null) { StopCoroutine(setActiveRoutine); }
        if (lookAtCamera && lookAtCameraScript == null) { lookAtCameraScript = gameObject.AddComponent<LookAtCamera>(); }

        //Without animation
        if (_instantaneously)
        {
            transform.localScale = _value ? openedScale : closedScale;
            if (lookAtCamera) { lookAtCameraScript.SetActive(_value); }
        }

        //With animation
        else { setActiveRoutine = StartCoroutine(IESetActive(_value)); }

    }

    private IEnumerator IESetActive(bool _value)
    {
        Vector3 _startValue = transform.localScale;
        AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        if (lookAtCamera && _value) { lookAtCameraScript.SetActive(true); }

        float _timeKey = 0;
        while (_timeKey < 1)
        {
            _timeKey += Time.deltaTime / lerpScaleDuration;
            float _evaluatedTimeKey = _curve.Evaluate(_timeKey);

            transform.localScale = Vector3.Lerp(_startValue, _value ? openedScale : closedScale, _evaluatedTimeKey);
            yield return null;
        }

        if (lookAtCamera && !_value) { lookAtCameraScript.SetActive(false); }

        yield return null;
    }
}
