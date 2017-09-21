using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

	[SerializeField]
	private RoomLayout _roomLayout;
	public RoomLayout RoomLayout{
		get { return _roomLayout; }
	}

	public void OnClickJoinRoom(string roomName){
		if (PhotonNetwork.JoinRoom (roomName)) {

		} else {
			print ("Join Room Failed");
		}
	}
}
