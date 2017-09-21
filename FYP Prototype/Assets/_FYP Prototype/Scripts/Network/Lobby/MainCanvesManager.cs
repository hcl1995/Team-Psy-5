using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainCanvesManager : MonoBehaviour
{

	public static MainCanvesManager Instance;

	[SerializeField]
	private LobbyCanvas _lobbyCanvas;
	public LobbyCanvas LobbyCanvas
	{
		get { return _lobbyCanvas; }
	}

	[SerializeField]
	private CurrentRoomCanvas _currentRoomCanvas;
	public CurrentRoomCanvas CurrentRoomCanvas
	{
		get { return _currentRoomCanvas; }
	}

	public GameObject lobbyBG;

	private void Awake()
	{
		Instance = this;
	}

	private void OnJoinedRoom()
	{
		lobbyBG.GetComponent<Image> ().enabled = false;
	}

	private void OnLeftRoom(){
		lobbyBG.GetComponent<Image> ().enabled = true;
	}
}
