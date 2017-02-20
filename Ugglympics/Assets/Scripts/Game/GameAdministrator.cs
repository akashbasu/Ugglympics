using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GameAdministrator : MonoBehaviour {

    private PlayerData playerData;
    private int gameSessions;

    void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        EventManager.Subscribe(Events.InputEvents.validSwipeCaptured, OnValidSwipe);
        EventManager.Subscribe(Events.InputEvents.validSwipeSetCaptured, OnValidSwipeSetCaptured);
        EventManager.Subscribe(Events.UIEvents.hitButtonClick, OnHitButtonClick);
        EventManager.Subscribe(Events.PlayerDataEvents.swipeMeterValueChanged, OnSwipeMeterValueChanged);
        EventManager.Subscribe(Events.UIEvents.resetButtonClick, OnSessionReset);
        EventManager.Subscribe(Events.UIEvents.cheatButtonClick, OnSessionReset);
    }

    void Start()
    {
        OnSessionReset(null);
    }

    void Destroy()
    {
        playerData = null;
        EventManager.Unsubscribe(Events.InputEvents.validSwipeCaptured, OnValidSwipe);
        EventManager.Unsubscribe(Events.InputEvents.validSwipeSetCaptured, OnValidSwipeSetCaptured);
        EventManager.Unsubscribe(Events.UIEvents.hitButtonClick, OnHitButtonClick);
        EventManager.Unsubscribe(Events.PlayerDataEvents.swipeMeterValueChanged, OnSwipeMeterValueChanged);
        EventManager.Unsubscribe(Events.UIEvents.resetButtonClick, OnSessionReset);
        EventManager.Unsubscribe(Events.UIEvents.cheatButtonClick, OnSessionReset);
    }

    IEnumerator SwipeMeterDecay()
    {
        yield return new WaitForSeconds(1f);
        playerData.SwipeMeter -= GameConstants.instance.decayPerSecond;
        StartCoroutine(SwipeMeterDecay());
    }

    IEnumerator HitMeterGrowth()
    {
        yield return new WaitForSeconds(1f);
        playerData.HitMeter += GameConstants.instance.growthPerSecond;
        StartCoroutine(HitMeterGrowth());
    }

    void OnValidSwipe(object[] args)
    {
        if (playerData.Swipes == 0)
            OnSessionStart();

        if (!playerData.IsStunned)
            playerData.Swipes++;
    }

    void OnValidSwipeSetCaptured(object[] args)
    {
        playerData.SwipeMeter += GameConstants.instance.swipeSetMultiplier;
    }

    void OnHitButtonClick(object[] args)
    {
        playerData.IsStunned = true;
        playerData.HitMeter = 0;
        StartCoroutine(OnStun());
    }

    IEnumerator OnStun()
    {
        yield return new WaitForSeconds(GameConstants.instance.stunTime);
        playerData.IsStunned = false;
    }
    
    void OnSwipeMeterValueChanged(object[] args)
    {
        if (args.Length == 0)
            return;

        if (playerData.SwipeMeter >= GameConstants.instance.swipeMeterMax)
        {
            playerData.GameTime = Time.time - playerData.StartTime;

            EventManager.SendMessage(Events.GameEvents.swipePhaseComplete, new object[1] { playerData });

            OnSessionReset(null);
        }
    }

    void OnSessionStart()
    {
        playerData.StartTime = Time.time;
        StartCoroutine(SwipeMeterDecay());
        StartCoroutine(HitMeterGrowth());
    }

    void OnSessionReset(object[] args)
    {
        StopAllCoroutines();
        playerData.Reset();
    }

}
