using UnityEngine;
using UnityEngine.UI;

public class HitMeter : MonoBehaviour
{

    public Slider slider;

    void OnEnable()
    {
        EventManager.Subscribe(Events.PlayerDataEvents.hitMeterValueChanged, UpdateValue);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(Events.PlayerDataEvents.hitMeterValueChanged, UpdateValue);
    }

    void UpdateValue(object[] args)
    {
        if (args.Length == 0)
            return;

        slider.value = (float)args[0];
        slider.maxValue = GameConstants.instance.hitMeterMax;
    }
}
