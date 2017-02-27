using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkManager {

    public Dictionary<int, PlayerModel> players = new Dictionary<int, PlayerModel>();

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        players.Add(conn.connectionId, conn.playerControllers[0].gameObject.GetComponent<PlayerModel>());
    }

    private void Update()
    {
        if(Input.GetKeyUp("space"))
        {
            foreach (KeyValuePair<int, PlayerModel> entry in players)
            {
                Debug.LogFormat("Connection ID : {0} || PlayerData : {1}", entry.Key, entry.Value.ToString());
            }
        }
    }
}
