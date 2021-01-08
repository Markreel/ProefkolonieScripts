using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject imageHolder;
    [SerializeField] private Image image;
    [SerializeField] private Image uncroppedImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void Activate(string _name, string _description, Sprite _sprite = null, bool _isEventItem = false)
    {
        imageHolder.SetActive(_sprite != null);

        image.gameObject.SetActive(!_isEventItem);
        uncroppedImage.gameObject.SetActive(_isEventItem);

        nameText.text = _name;
        descriptionText.text = _description;
        image.sprite = uncroppedImage.sprite = _sprite;

        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, true);
    }

    public void Deactivate()
    {
        GameManager.Instance.UIManager.SetActiveCanvasGroup(canvasGroup, false);
    }
}
