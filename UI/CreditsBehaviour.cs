using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsBehaviour : MonoBehaviour
{
    [Header("Roll Credits Settings: ")]
    [SerializeField] float rollCreditsSlideDuration;
    [SerializeField] float rollCreditsTextDuration;
    [SerializeField] float rollCreditsTextDelay;
    [Space]
    [SerializeField] private CanvasGroup rollCreditsCanvasGroup;
    [SerializeField] private GameObject rollCreditsText;
    [Space]
    [SerializeField] Vector3 rollCreditsPositionA;
    [SerializeField] Vector3 rollCreditsPositionB;
    [SerializeField] Vector3 rollCreditsTextPositionA;
    [SerializeField] Vector3 rollCreditsTextPositionB;

    [Header("Meer Weten?")]
    [SerializeField] private GameObject meerWetenText;
    [SerializeField] private Vector3 meerWetenPositionA;
    [SerializeField] private Vector3 meerWetenPositionB;

    [Header("Segments Settings: ")]
    [SerializeField] private float fadeDuration;
    [SerializeField] Vector3 segmentsPositionA;
    [SerializeField] Vector3 segmentsPositionB;
    [SerializeField] Vector3 segmentsScaleA = Vector3.one;
    [SerializeField] Vector3 segmentsScaleB;
    [Space]
    [SerializeField] private CanvasGroup segmentsCanvasGroup;
    [Space]
    [SerializeField] private Image image;
    [Space]
    [SerializeField] private Transform lineParent;
    [SerializeField] private GameObject linePrefab;
    private List<TextMeshProUGUI> currentLines = new List<TextMeshProUGUI>();

    [SerializeField] private List<CreditsSegment> segments = new List<CreditsSegment>();

    private int currentSegmentIndex;

    private Coroutine rollCreditsRoutine;
    private Coroutine displayCurrentSegmentRoutine;

    private void Start()
    {
        GameManager.Instance.UIManager.SetActiveCanvasGroup(segmentsCanvasGroup, false);
    }

    public void StartCredits()
    {
        GetComponent<Canvas>().enabled = true;
        GameManager.Instance.UIManager.SetActiveCanvasGroup(segmentsCanvasGroup, true);

        currentSegmentIndex = 0;
        DisplayCurrentSegment();

        GameManager.Instance.AudioManager.PlayCreditsMusic();
    }

    private void NextSegment()
    {
        currentSegmentIndex++;
        if (currentSegmentIndex >= segments.Count) { StopCredits(); return; }

        DisplayCurrentSegment();
    }

    private void StopCredits()
    {
        RollCredits();
    }

    private void DisplayCurrentSegment()
    {
        if (displayCurrentSegmentRoutine != null) { StopCoroutine(displayCurrentSegmentRoutine); }
        displayCurrentSegmentRoutine = StartCoroutine(IEDisplayCurrentSegment());
    }

    private IEnumerator IEDisplayCurrentSegment()
    {
        //Setup variables
        float _lerpTime = 0;
        CreditsSegment _segment = segments[currentSegmentIndex];

        //Add lines to scene and save them in the "currentLines" list
        foreach (var _lines in _segment.Lines)
        {
            GameObject _obj = Instantiate(linePrefab, lineParent);
            TextMeshProUGUI _tmp = _obj.GetComponent<TextMeshProUGUI>();

            _tmp.color = Color.clear;
            _tmp.text = _lines.Text;
            currentLines.Add(_tmp);
        }

        //Replace image if it is not the same
        if (image.sprite != _segment.Sprite)
        {
            //Fade out image if it is not empty
            if (image.sprite != null)
            {
                while (_lerpTime < 1)
                {
                    _lerpTime += Time.unscaledDeltaTime / (fadeDuration / 2);

                    image.color = Color.Lerp(Color.white, Color.clear, _lerpTime);
                    yield return null;
                }
            }

            //Replace the sprite of the image with the new sprite
            image.sprite = _segment.Sprite;

            //Fade int image
            _lerpTime = 0f;
            while (_lerpTime < 1)
            {
                _lerpTime += Time.unscaledDeltaTime / (fadeDuration / 2);

                image.color = Color.Lerp(Color.clear, Color.white, _lerpTime);
                yield return null;
            }
        }

        //Fade in each line one by one
        foreach (var _line in _segment.Lines)
        {
            //Wait before displaying the line
            yield return new WaitForSecondsRealtime(_line.DisplayDelay);

            //Fade in line
            _lerpTime = 0;
            while (_lerpTime < 1)
            {
                _lerpTime += Time.unscaledDeltaTime / fadeDuration;

                currentLines[_segment.Lines.IndexOf(_line)].color = Color.Lerp(Color.clear, Color.white, _lerpTime);
                yield return null;
            }
        }

        //Wait before fading out the lines
        yield return new WaitForSecondsRealtime(_segment.DisplayDuration);

        //Fade out all lines at once
        _lerpTime = 0;
        while (_lerpTime < 1)
        {
            _lerpTime += Time.unscaledDeltaTime / fadeDuration;

            foreach (var _line in currentLines)
            {
                _line.color = Color.Lerp(Color.white, Color.clear, _lerpTime);
            }

            yield return null;
        }

        //Remove lines from the scene and clear the "currentLines" list
        foreach (var _line in currentLines)
        {
            Destroy(_line.gameObject);
        }
        currentLines.Clear();

        //End the Coroutine and start the next segment
        displayCurrentSegmentRoutine = null;
        NextSegment();
        yield return null;
    }

    private void RollCredits()
    {
        if (rollCreditsRoutine != null) { StopCoroutine(rollCreditsRoutine); }
        rollCreditsRoutine = StartCoroutine(IERollCredits());
    }

    private IEnumerator IERollCredits()
    {
        //Fade out music
        GameManager.Instance.AudioManager.FadeMusic();

        //Slide the credits in from the side
        float _timeKey = 0f;
        while (_timeKey < 1f)
        {
            _timeKey += Time.unscaledDeltaTime / rollCreditsSlideDuration;

            rollCreditsCanvasGroup.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(rollCreditsPositionA, rollCreditsPositionB, _timeKey);

            segmentsCanvasGroup.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(segmentsPositionA, segmentsPositionB, _timeKey);
            segmentsCanvasGroup.GetComponent<RectTransform>().localScale = Vector3.Lerp(segmentsScaleA, segmentsScaleB, _timeKey);

            meerWetenText.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(meerWetenPositionA, meerWetenPositionB, _timeKey);

            yield return null;
        }

        //Wait before rolling the credits so the first lines are easier to read
        yield return new WaitForSecondsRealtime(rollCreditsTextDelay);
        GameManager.Instance.AudioManager.PlayRollCreditsMusic();

        //Roll the credits
        _timeKey = 0f;
        while (_timeKey < 1f)
        {
            _timeKey += Time.unscaledDeltaTime / rollCreditsTextDuration;

            rollCreditsText.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(rollCreditsTextPositionA, rollCreditsTextPositionB, _timeKey);

            yield return null;
        }

        //Fade out music and wait for it to fade out
        GameManager.Instance.AudioManager.FadeMusic();
        yield return new WaitForSecondsRealtime(3);

        //Delete the save file and quit the application
        rollCreditsRoutine = null;
        SaveSystem.DeleteSaveFile();
        Application.Quit();

        yield return null;
    }
}

[System.Serializable]
public class CreditsSegment
{
    public Sprite Sprite;
    public float DisplayDuration;
    public List<CreditsLine> Lines = new List<CreditsLine>();
}

[System.Serializable]
public class CreditsLine
{
    [TextArea]
    public string Text;
    public float DisplayDelay;
}
