using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMeter : MonoBehaviour {

    public Slider slider;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        EventManager.Subscribe(Events.PlayerDataEvents.swipeMeterValueChanged, UpdateValue);
        EventManager.Subscribe(Events.PlayerDataEvents.stunStateChanged, OnStunStateChanged);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(Events.PlayerDataEvents.swipeMeterValueChanged, UpdateValue);
        EventManager.Unsubscribe(Events.PlayerDataEvents.stunStateChanged, OnStunStateChanged);
    }

    void ToggleSlider(bool enable)
    {
        canvasGroup.alpha = enable ? 1 : 0;
    }

    void UpdateValue(object [] args)
    {
        if (args.Length == 0)
            return;

        slider.value = (float)args[0];
        slider.maxValue = GameConstants.instance.swipeMeterMax;
    }

    void OnStunStateChanged(object[] args)
    {
        if (args.Length == 0)
            return;

        bool isStunned = (bool)args[0];
        if (isStunned)
            StartCoroutine(OnStunned());
        else
        {
            StopAllCoroutines();
            canvasGroup.alpha = 1;
        }
    }

    IEnumerator OnStunned()
    {
        ToggleSlider(canvasGroup.alpha == 1 ? false : true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(OnStunned());
    }
}
