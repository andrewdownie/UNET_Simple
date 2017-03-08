using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Net_Manager : NetworkManager {
	[SerializeField]
	private GameObject player_Prefab;


	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(player_Prefab);
		NetworkServer.Spawn(newPlayer);
		NetworkIdentity newIdentity = newPlayer.GetComponent<NetworkIdentity>();
		newIdentity.AssignClientAuthority(conn);
	}

	public override void OnServerReady(NetworkConnection conn){
		NetworkServer.SetClientReady(conn);
		Debug.Log("Server is now ready");
		NetworkServer.SetClientReady(conn);
		
	}
}
