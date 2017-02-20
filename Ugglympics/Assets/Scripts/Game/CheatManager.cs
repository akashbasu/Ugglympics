using System.Collections.Generic;
using System.Reflection;

class CheatManager
{
    private static Dictionary<string, object> cheatList;
    private static FieldInfo[] fields;

    public static Dictionary<string, object> GetCheatList()
    {
        if (cheatList == null)
        {
            cheatList = new Dictionary<string, object>();
            PopulateCheats(null);
            EventManager.Subscribe(Events.CheatEvents.incrementCheatValue, OnCheatValueIncreased);
            EventManager.Subscribe(Events.CheatEvents.decrementCheatValue, OnCheatValueDecreased);
            EventManager.Subscribe(Events.CheatEvents.refreshCheats, PopulateCheats);
        }

        return cheatList;
    }

    private static void PopulateCheats(object [] args)
    {
        fields = typeof(GameConstants).GetFields();
        foreach (FieldInfo field in fields)
        {
            if (cheatList.ContainsKey(field.Name))
                cheatList[field.Name] = field.GetValue(GameConstants.instance);
            else
                cheatList.Add(field.Name, field.GetValue(GameConstants.instance));
        }
    }

    private static void OnCheatValueIncreased(object[] args)
    {
        if (args.Length == 0)
            return;

        string key = (string)args[0];
        float value = (float)cheatList[key];
        value += GameConstants.instance.delta;
        cheatList[key] = value;
        typeof(GameConstants).GetField(key).SetValue(GameConstants.instance, value);

        EventManager.SendMessage(Events.CheatEvents.refreshCheats, null);
    }

    private static void OnCheatValueDecreased(object[] args)
    {
        if (args.Length == 0)
            return;

        string key = (string)args[0];
        float value = (float)cheatList[key];
        value -= GameConstants.instance.delta;
        cheatList[key] = value;
        typeof(GameConstants).GetField(key).SetValue(GameConstants.instance, value);

        EventManager.SendMessage(Events.CheatEvents.refreshCheats, null);
    }
}
