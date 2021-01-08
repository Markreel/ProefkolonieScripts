using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoldOutElement : MonoBehaviour
{
    [SerializeField] Vector3 foldedOutPos;
    [SerializeField] Vector3 foldedInPos;
    [SerializeField] Button foldInButton;
    [Space]
    [SerializeField] bool foldedOutOnAwake;
    [SerializeField] float foldDuration;
    [SerializeField] AnimationCurve foldCurve;

    private RectTransform rectTransform;
    private bool foldedOut;

    private float lerpValue = 0;
    private Coroutine foldRoutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = foldedOutOnAwake ? foldedOutPos : foldedInPos;
        if (foldedOutOnAwake) { foldedOut = true; lerpValue = 1; foldInButton.transform.localScale = new Vector3(1, -1, 1); }
    }

    private IEnumerator IEFold(bool _inverse = false)
    {
        if (!_inverse)
        {
            foldedOut = true;

            while (lerpValue < 1f)
            {
                lerpValue += Time.unscaledDeltaTime / foldDuration;
                float _evaluatedLerpValue = foldCurve.Evaluate(lerpValue);

                rectTransform.anchoredPosition = Vector3.Lerp(foldedInPos, foldedOutPos, _evaluatedLerpValue);
                foldInButton.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1, -1, 1), _evaluatedLerpValue);

                yield return null;
            }
        }

        else
        {
            foldedOut = false;

            while (lerpValue > 0f)
            {
                lerpValue -= Time.deltaTime / foldDuration;
                float _evaluatedLerpValue = foldCurve.Evaluate(lerpValue);

                rectTransform.anchoredPosition = Vector3.Lerp(foldedInPos, foldedOutPos, _evaluatedLerpValue);
                foldInButton.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1, -1, 1), _evaluatedLerpValue);

                yield return null;
            }
        }
    }

    public void FoldOut(bool _instant = false)
    {
        if (_instant) { lerpValue = 1; }

        if (foldRoutine != null) StopCoroutine(foldRoutine);
        foldRoutine = StartCoroutine(IEFold());
    }

    public void FoldIn(bool _instant = false)
    {
        if (_instant) { lerpValue = 0; }

        if (foldRoutine != null) StopCoroutine(foldRoutine);
        foldRoutine = StartCoroutine(IEFold(true));
    }

    public void ToggleFold()
    {
        if (foldedOut) { FoldIn(); }
        else { FoldOut(); }
    }
}
