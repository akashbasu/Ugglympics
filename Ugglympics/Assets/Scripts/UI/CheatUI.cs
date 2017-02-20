using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CheatUI : MonoBehaviour
{
    public Button cheatButton;
    public Button gameButton;
    public GameObject gameElements;
    public GameObject cheatElements;
    public GameObject sampleRow;
    public GameObject cheatGroup;

    private bool isCheatMenuShowing = false;

    void Awake()
    {
        CreateCheatMenu();
        EventManager.Subscribe(Events.CheatEvents.refreshCheats, RefreshCheats);
    }

    public void ToggleCheats()
    {
        isCheatMenuShowing = !isCheatMenuShowing;
        EventManager.SendMessage(Events.UIEvents.cheatButtonClick, null);

        if (isCheatMenuShowing)
            RefreshCheats(null);

        gameElements.GetComponent<Canvas>().enabled = !isCheatMenuShowing;
        cheatElements.GetComponent<Canvas>().enabled = isCheatMenuShowing;
    }

    void CreateCheatMenu()
    {
        foreach (KeyValuePair<string, object> row in CheatManager.GetCheatList())
        {
            GameObject cheatRow = Instantiate(sampleRow);
            cheatRow.GetComponent<CheatRow>().Setup(row.Key, row.Value);
            cheatRow.transform.parent = cheatGroup.transform;
            cheatRow.transform.localScale = sampleRow.transform.localScale;
            cheatRow.SetActive(true);
        }

        Destroy(sampleRow);
    }

    void RefreshCheats(object[] args)
    {
        foreach (CheatRow row in cheatGroup.GetComponentsInChildren<CheatRow>())
            row.Setup(row.Key, CheatManager.GetCheatList()[row.Key]);
    }
}
