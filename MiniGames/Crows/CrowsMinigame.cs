using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrowsMinigame : MonoBehaviour, IMiniGame
{
    [SerializeField] TextMeshProUGUI explanationText;

    [SerializeField] int minCrows = 3;
    [SerializeField] Sprite crowScaredSprite;
    [SerializeField] Sprite easterEggCrowScaredSprite;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private List<GameObject> possibleCrows = new List<GameObject>();

    [SerializeField] private AudioClip[] audioWhenCrowClicked;
    [SerializeField] private AudioClip[] easterEggAudioWhenCrowClicked;

    [SerializeField] private List<Sprite> originalCrowSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> easterEggOriginalCrowSprites = new List<Sprite>();

    private GameObject crowsThatActivatedTheMinigame;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        foreach (var _crow in possibleCrows)
        {
            originalCrowSprites.Add(_crow.GetComponent<Image>().sprite);
        }
    }

    private void Start()
    {
        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, false);
    }

    private void RandomizeCrows()
    {
        int _amount = Random.Range(minCrows, possibleCrows.Count);

        //Shuffle possible crows
        for (int i = 0; i < possibleCrows.Count; i++)
        {
            var _temp = possibleCrows[i];
            var _tempSprite = originalCrowSprites[i];

            int _randomIndex = Random.Range(i, possibleCrows.Count);

            possibleCrows[i] = possibleCrows[_randomIndex];
            originalCrowSprites[i] = originalCrowSprites[_randomIndex];

            possibleCrows[_randomIndex] = _temp;
            originalCrowSprites[_randomIndex] = _tempSprite;
        }

        //Assign the original sprites to each crow and reactivate the buttons
        for (int i = 0; i < possibleCrows.Count; i++)
        {
            possibleCrows[i].GetComponent<Image>().sprite = originalCrowSprites[i];
            possibleCrows[i].GetComponent<Button>().interactable = true;
        }

        //Activate the given amount of crows
        for (int i = 0; i < possibleCrows.Count; i++)
        {
            possibleCrows[i].SetActive(i < _amount);
        }
    }

    private void CheckIfGameIsWon()
    {
        foreach (var _crow in possibleCrows)
        {
            if (_crow.activeInHierarchy) { return; }
        }

        WinMiniGame();
    }

    public void DeactivateCrow(GameObject _obj)
    {
        StartCoroutine(IEDeactivateCrow(_obj));
    }

    private IEnumerator IEDeactivateCrow(GameObject _obj)
    {
        _obj.GetComponent<Image>().sprite = crowScaredSprite;
        _obj.GetComponent<Button>().interactable = false;
        GameManager.Instance.AudioManager.PlayRandomClip(audioWhenCrowClicked);

        yield return new WaitForSecondsRealtime(0.5f);

        _obj.SetActive(false);
        CheckIfGameIsWon();

        yield return null;
    }

    public void StartMiniGame(GameObject _crowsThatActivatedTheMinigame)
    {
        crowsThatActivatedTheMinigame = _crowsThatActivatedTheMinigame;
        RandomizeCrows();
        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, true, fadeRoutine);
    }

    public void LoseMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public void WinMiniGame()
    {
        crowsThatActivatedTheMinigame?.SetActive(false);
        crowsThatActivatedTheMinigame = null;

        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, false, fadeRoutine);
    }

    public void ChangeCrowsToLeprechauns()
    {
        crowScaredSprite = easterEggCrowScaredSprite;
        originalCrowSprites = easterEggOriginalCrowSprites;
        audioWhenCrowClicked = easterEggAudioWhenCrowClicked;
        explanationText.text = "Jaag de kabouters weg!";
    }
}