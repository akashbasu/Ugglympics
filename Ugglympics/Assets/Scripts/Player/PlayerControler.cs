using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerControler : NetworkBehaviour {

    private PlayerModel playerModel = null;

	public override void OnStartLocalPlayer()
    {
    	base.OnStartLocalPlayer ();

    	playerModel = gameObject.GetComponent <PlayerModel> ();
        if (playerModel != null)
        {
            AddGameEventListeners();
            OnSessionReset(null);
        }
    }

    void AddGameEventListeners()
    {
        EventManager.Subscribe(Events.InputEvents.validSwipeCaptured, OnValidSwipe);
        EventManager.Subscribe(Events.InputEvents.validSwipeSetCaptured, OnValidSwipeSetCaptured);
        EventManager.Subscribe(Events.PlayerDataEvents.swipeMeterValueChanged, OnSwipeMeterValueChanged);
        EventManager.Subscribe(Events.UIEvents.resetButtonClick, OnSessionReset);
        EventManager.Subscribe(Events.UIEvents.cheatButtonClick, OnSessionReset);
    }

    void Destroy()
    {
        playerModel = null;
        EventManager.Unsubscribe(Events.InputEvents.validSwipeCaptured, OnValidSwipe);
        EventManager.Unsubscribe(Events.InputEvents.validSwipeSetCaptured, OnValidSwipeSetCaptured);
        EventManager.Unsubscribe(Events.PlayerDataEvents.swipeMeterValueChanged, OnSwipeMeterValueChanged);
        EventManager.Unsubscribe(Events.UIEvents.resetButtonClick, OnSessionReset);
        EventManager.Unsubscribe(Events.UIEvents.cheatButtonClick, OnSessionReset);
    }

    IEnumerator SwipeMeterDecay()
    {
        yield return new WaitForSeconds(1f);
        playerModel.SwipeMeter -= GameConstants.instance.decayPerSecond;
        StartCoroutine(SwipeMeterDecay());
    }

    IEnumerator HitMeterGrowth()
    {
        yield return new WaitForSeconds(1f);
        playerModel.HitMeter += GameConstants.instance.growthPerSecond;
        StartCoroutine(HitMeterGrowth());
    }

    void OnValidSwipe(object[] args)
    {
        if (playerModel.Swipes == 0)
            OnSessionStart();

        if (!playerModel.IsStunned)
            playerModel.Swipes++;
    }

    void OnValidSwipeSetCaptured(object[] args)
    {
        playerModel.SwipeMeter += GameConstants.instance.swipeSetMultiplier;
    }

    IEnumerator OnStun()
    {
		playerModel.IsStunned = true;
        yield return new WaitForSeconds(GameConstants.instance.stunTime);
        playerModel.IsStunned = false;
    }
    
    void OnSwipeMeterValueChanged(object[] args)
    {
        if (args.Length == 0)
            return;

        if (playerModel.SwipeMeter >= GameConstants.instance.swipeMeterMax)
        {
            playerModel.GameTime = Time.time - playerModel.StartTime;

            EventManager.SendMessage(Events.GameEvents.swipePhaseComplete, new object[1] { playerModel });

            OnSessionReset(null);
        }
    }

    void OnSessionStart()
    {
        playerModel.StartTime = Time.time;
        StartCoroutine(SwipeMeterDecay());
        StartCoroutine(HitMeterGrowth());
    }

    void OnSessionReset(object[] args)
    {
        StopAllCoroutines();
        playerModel.Reset();
    }

}
