using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipmentMinigame : MonoBehaviour, IMiniGame
{
    [SerializeField] private float progressPerClick = 0.1f;
    [Space]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image progressBar;
    [SerializeField] private Button button;

    private GameObject ship;

    private float previousProgress;
    private float currentProgress;

    private Coroutine fadeRoutine;
    private Coroutine progressRoutine;

    private void Start()
    {
        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, false);
    }

    private void LerpProgressBar(float _t)
    {
        progressBar.fillAmount = Mathf.Lerp(previousProgress, currentProgress, _t);
    }

    private void ResetMiniGame()
    {
        progressRoutine = null;
        button.interactable = true;
        progressBar.fillAmount = previousProgress = currentProgress = 0;
    }

    private void CheckIfWon()
    {
        if(currentProgress  >= 1)
        {
            button.interactable = false;
            GameManager.Instance.CoroutineHandler.LerpOverTime("ShipmentMiniGameWon", 1f, null, null, WinMiniGame, null, true);
        }
    }

    public void Progress()
    {
        currentProgress = Mathf.Clamp(currentProgress + progressPerClick, 0f, 1f);
        previousProgress = progressBar.fillAmount;
        GameManager.Instance.CoroutineHandler.StartRoutine(ref progressRoutine, IELerpProgressBar());

        CheckIfWon();
    }

    public void StartMiniGame(GameObject _objectThatActivatedTheMinigame)
    {
        ship = _objectThatActivatedTheMinigame;
        ResetMiniGame();
        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, true, fadeRoutine);
    }

    public void LoseMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public void WinMiniGame()
    {
        if(ship != null)
        {
            ship.GetComponent<Ship>().CompletedShipmentPreviousDay = true;
            ship.SetActive(false);
            ship = null;
        }

        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, false, fadeRoutine);
    }

    private IEnumerator IELerpProgressBar()
    {
        float _timeKey = 0f;

        while (_timeKey < 1f)
        {
            _timeKey += Time.unscaledDeltaTime / 0.25f;
            float _evaluatedTimeKey = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(_timeKey);

            LerpProgressBar(_evaluatedTimeKey);
            yield return null;
        }

        progressRoutine = null;
        yield return null;
    }
}
