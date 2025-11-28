using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TowerShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public System.Action OnHoverEnter;
    public System.Action OnHoverExit;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text buttonText;

    private Button myButton;

    public void AddOnClickEvent(System.Action onClickEvent)
    {
        if(myButton == null)
        {
            myButton = GetComponent<Button>();
        }
        myButton.onClick.AddListener(onClickEvent.Invoke);
    }

    public void SetButtonText(string newText)
    {
        buttonText.text = newText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke();
    }

    public void SetImageGraphic(Sprite graphic)
    {
        iconImage.sprite = graphic;
    }
}
