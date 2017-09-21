using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;
	public string PlayerName { get; private set; }
	private PhotonView PhotonView;
	private int PlayersInGame = 0;

	private PlayerControl02 CurrentPlayer;

	// Use this for initialization
	private void Awake()
	{
		Instance = this;
		PhotonView = GetComponent<PhotonView>();

		PlayerName = "Player#" + Random.Range(1000, 9999);
		PhotonNetwork.playerName = PlayerName;
		PhotonNetwork.sendRate = 60;
		PhotonNetwork.sendRateOnSerialize = 30;

		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (PhotonNetwork.isMasterClient)
			MasterLoadedGame();
		else
			NonMasterLoadedGame();
		
	}

	private void MasterLoadedGame()
	{
		PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
		PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
	}

	private void NonMasterLoadedGame()
	{
		PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
	}

	[PunRPC]
	private void RPC_LoadGameOthers()
	{
		PhotonNetwork.LoadLevel(1);
	}

	[PunRPC]
	private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
	{
		//PlayerManagement.Instance.AddPlayerStats(photonPlayer);

		PlayersInGame++;
		if (PlayersInGame == PhotonNetwork.playerList.Length)
		{
			print("All players are in the game scene.");
			PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
		}
	}

	public void NewHealth(PhotonPlayer photonPlayer, int health)
	{
		PhotonView.RPC("RPC_NewHealth", photonPlayer, health);
	}

	[PunRPC]
	private void RPC_NewHealth(int health)
	{
//		if (CurrentPlayer == null)
//			return;
//
//		if (health <= 0)
//			PhotonNetwork.Destroy(CurrentPlayer.gameObject);
//		else
//			CurrentPlayer.Health = health;
	}

	[PunRPC]
	private void RPC_CreatePlayer()
	{
		float randomValue = Random.Range(-10.0f, 10.0f);
		float randomValue2 = Random.Range(0.0f, 5.0f);
		GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "unitychan"), new Vector3(randomValue2,0.0f,randomValue), Quaternion.identity, 0);
		CurrentPlayer = obj.GetComponent<PlayerControl02>();
	}
}


