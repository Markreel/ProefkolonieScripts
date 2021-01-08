using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialBehaviour : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private Button skipButton;

    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<Vector3> buttonPosition;

    private int index = 0;

    private void Awake()
    {
        button.onClick.AddListener(NextImage);
        DisplayCurrentImage();
    }

    public void NextImage()
    {
        skipButton.gameObject.SetActive(false);

        index++;
        if(index >= sprites.Count) { StopTutorial(); return; }

        DisplayCurrentImage();
    }

    public void StopTutorial()
    {
        Destroy(gameObject);
    }

    public void DisplayCurrentImage()
    {
        image.sprite = sprites[index];
        button.GetComponent<RectTransform>().anchoredPosition = buttonPosition[index];
    }
}
