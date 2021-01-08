using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CoroutineCheckMethod { StopExistingRoutine, WaitForExistingRoutine }

public class CoroutineHandler : MonoBehaviour
{
    public static Dictionary<string, CoroutineData> Coroutines = new Dictionary<string, CoroutineData>();


    public void LerpOverTime(string _key, float _duration, UnityAction<float> _onUpdate, UnityAction _onStart = null, UnityAction _onFinished = null, AnimationCurve _curve = null,
        bool _ignoreTimeScale = false, CoroutineCheckMethod _checkMethod = CoroutineCheckMethod.StopExistingRoutine)
    {
        CoroutineData _coroutineData = new CoroutineData();
        _coroutineData.Key = _key;
        _coroutineData.OnStart = _onStart;
        _coroutineData.OnUpdate = _onUpdate;
        _coroutineData.OnFinished = _onFinished;

        LerpOverTime(_coroutineData, _duration, _curve, _ignoreTimeScale, _checkMethod);
    }

    public void LerpOverTime(CoroutineData _coroutineData, float _duration, AnimationCurve _curve = null, 
        bool _ignoreTimeScale = false, CoroutineCheckMethod _checkMethod = CoroutineCheckMethod.StopExistingRoutine)
    {

        switch (_checkMethod)
        {
            default:
            case CoroutineCheckMethod.StopExistingRoutine:

                if (Coroutines.ContainsKey(_coroutineData.Key))
                {
                    if (Coroutines[_coroutineData.Key].Routine != null) { StopCoroutine(Coroutines[_coroutineData.Key].Routine); }  
                    Coroutines.Remove(_coroutineData.Key);
                }

                Coroutines.Add(_coroutineData.Key, _coroutineData);

                StartCoroutine(IELerpOverTime(_coroutineData, _duration, _curve, _ignoreTimeScale));

                break;
            case CoroutineCheckMethod.WaitForExistingRoutine:

                if (!Coroutines.ContainsKey(_coroutineData.Key))
                {
                    Coroutines.Add(_coroutineData.Key, _coroutineData);
                    _coroutineData.Routine = StartCoroutine(IELerpOverTime(_coroutineData, _duration, _curve, _ignoreTimeScale));
                }

                break;
        }
    }

    public IEnumerator IELerpOverTime(CoroutineData _coroutineData, float _duration, AnimationCurve _curve = null, bool _ignoreTimeScale = false)
    {
        if (_coroutineData.OnStart != null) { _coroutineData.OnStart.Invoke(); }

        if (_curve == null) { _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); }


        float _lerpTime = 0f;
        while (_lerpTime < 1f)
        {
            float _time = _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;   //fix dit door er monobehaviour van te maken i guess?
            _lerpTime += _time / _duration;

            float _evaluatedLerpTime = _curve.Evaluate(_lerpTime);

            if (_coroutineData.OnUpdate != null) { _coroutineData.OnUpdate.Invoke(_evaluatedLerpTime); }
            yield return null;
        }

        if (_coroutineData.OnUpdate != null) { _coroutineData.OnUpdate.Invoke(1f); }
        if (_coroutineData.OnFinished != null) { _coroutineData.OnFinished.Invoke(); }

        if (Coroutines.ContainsKey(_coroutineData.Key)) { Coroutines.Remove(_coroutineData.Key); }

        yield return null;
    }

    public void StartRoutine(ref Coroutine _coroutine, IEnumerator _ienumerator, CoroutineCheckMethod _checkMethod = CoroutineCheckMethod.StopExistingRoutine)
    {
        switch (_checkMethod)
        {
            default:
            case CoroutineCheckMethod.StopExistingRoutine:
                if(_coroutine != null) { StopCoroutine(_coroutine); }
                _coroutine = StartCoroutine(_ienumerator);
                break;
            case CoroutineCheckMethod.WaitForExistingRoutine:
                if (_coroutine == null) { _coroutine = StartCoroutine(_ienumerator); }         
                break;
        }
    }
}

public class CoroutineData
{
    public string Key;
    public UnityAction<float> OnUpdate;
    public UnityAction OnStart;
    public UnityAction OnFinished;
    public Coroutine Routine;
}

