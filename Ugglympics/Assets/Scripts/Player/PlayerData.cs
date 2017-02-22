using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
    public int swipes;
    public float swipeMeter;
    public float hitMeter;
    public float startTime;
    public float gameTime;
    public int stunCount;

    [SyncVar]
    public bool isStunned;

    public int Swipes
    {
        get { return swipes; }
        set
        {
            if ((value > 0) && (value > swipes))
            {
                swipes = value;
                if (swipes % 2 == 0)
                    EventManager.SendMessage(Events.InputEvents.validSwipeSetCaptured, null);
            }
            else
                swipes = value;
        }
    }

    public float SwipeMeter
    {
        get { return swipeMeter; }
        set
        {
            if (value == swipeMeter)
                return;

            swipeMeter = (value < 0) ? 0 : value;

            EventManager.SendMessage(Events.PlayerDataEvents.swipeMeterValueChanged, new object[1] { swipeMeter });
        }
    }

    public float HitMeter
    {
        get { return hitMeter; }
        set
        {
            if (value == hitMeter)
                return;
            
            hitMeter = value >= GameConstants.instance.hitMeterMax ? GameConstants.instance.hitMeterMax : value;

            EventManager.SendMessage(Events.PlayerDataEvents.hitMeterValueChanged, new object[1] { hitMeter });
        }
    }

    public bool IsStunned
    {
        get { return isStunned; }
        set
        {
            if (isStunned == value)
                return;

            isStunned = value;
            stunCount = isStunned ? (stunCount + 1) : stunCount;

            EventManager.SendMessage(Events.PlayerDataEvents.stunStateChanged, new object[1] { isStunned });
        }
    }

    public float StartTime
    {
        get { return startTime; }
        set { startTime = value; }
    }

    public float GameTime
    {
        get { return gameTime; }
        set { gameTime = value; }
    }

    public int StunCount
    {
        get { return stunCount; }
        set { stunCount = value; }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        EventManager.SendMessage(Events.GameEvents.playerConnected, new object[1] { this});
    }

    public void  Reset()
    {
        Swipes = 0;
        SwipeMeter = 0;
        HitMeter = 0;
        IsStunned = false;
        StartTime = 0;
        GameTime = 0;
        StunCount = 0;
    }
}
