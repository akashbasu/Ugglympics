using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HitButton : MonoBehaviour
{
    public Button button;
    private CanvasGroup canvasGroup;

    void OnEnable()
    {
        EventManager.Subscribe(Events.PlayerDataEvents.hitMeterValueChanged, OnHitMeterValueChanged);
        button.onClick.AddListener(OnClick);

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        ToggleButton(false);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(Events.PlayerDataEvents.hitMeterValueChanged, OnHitMeterValueChanged);
        button.onClick.RemoveListener(OnClick);
    }

    void ToggleButton(bool enable)
    {
        canvasGroup.alpha = enable ? 1 : 0;
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;
    }

    void OnHitMeterValueChanged(object[] args)
    {
        if (args.Length == 0)
            return;

        float meterValue = (float)args[0];
        ToggleButton(meterValue == GameConstants.instance.hitMeterMax);
    }

    public void OnClick()
    {
        EventManager.SendMessage(Events.UIEvents.hitButtonClick, null);
        ToggleButton(false);
    }
}

