using UnityEngine;

public class ResetButton : MonoBehaviour {

    public void OnClick()
    {
        EventManager.SendMessage(Events.UIEvents.resetButtonClick, null);
    }
}
