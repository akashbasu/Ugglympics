using UnityEngine;
using UnityEngine.UI;

class CheatRow : MonoBehaviour
{
    private string key;
    private object value;

    public string Key { get { return key; } }

    public void Setup(string key, object value)
    {
        this.key = key;
        this.value = value;

        gameObject.transform.GetChild(0).GetComponent<Text>().text = key;
        gameObject.transform.GetChild(2).GetComponent<Text>().text = ((float)value).ToString();
    }

    public void OnIncrementValue()
    {
        EventManager.SendMessage(Events.CheatEvents.incrementCheatValue, new object[1] { key});
    }

    public void OnDecrementValue()
    {
        EventManager.SendMessage(Events.CheatEvents.decrementCheatValue, new object[1] { key });
    }
}