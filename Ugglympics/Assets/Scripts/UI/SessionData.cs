using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SessionData : MonoBehaviour {

    public GameObject row;
    private List<GameObject> rows;
    private int i;
    
    void OnEnable()
    {
        EventManager.Subscribe(Events.GameEvents.swipePhaseComplete, ShowSessionData);
        EventManager.Subscribe(Events.UIEvents.resetButtonClick, OnSessionReset);
        i = 0;
        rows = new List<GameObject>();
        rows.Add(row);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(Events.GameEvents.swipePhaseComplete, ShowSessionData);
        EventManager.Unsubscribe(Events.UIEvents.resetButtonClick, OnSessionReset);
    }

    void ShowSessionData(object[] args)
    {
        if (args.Length == 0)
            return;

        PlayerData data = args[0] as PlayerData;
        string output = String.Format("Player: {0}\nValid Swipes: {1}\nGame Time: {2} seconds\nStun Count: {3}\n", (i + 1), data.Swipes, data.GameTime, data.StunCount);

        if(rows.Count >= (i + 1))
        {
            rows[i].GetComponent<Text>().text = output;
        }
        else
        {
            GameObject rowCopy = Instantiate(row, new Vector3(row.transform.position.x, (row.transform.position.y - (row.GetComponent<RectTransform>().rect.height * 2))), row.transform.localRotation) as GameObject;
            rowCopy.transform.parent = gameObject.transform;
            rowCopy.transform.localScale = row.transform.localScale;
            rowCopy.GetComponent<Text>().text = output;
            rows.Add(rowCopy);
            row = rows[rows.Count - 1];
        }

        i++;
    }

    void OnSessionReset(object[] args)
    {
        foreach (GameObject row in rows)
        {
            row.GetComponent<Text>().text = "";
        }
        i = 0;
    }
}
