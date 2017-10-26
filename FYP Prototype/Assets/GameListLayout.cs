using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameListLayout : MonoBehaviour {
	[SerializeField]
	private GameObject _gameListingPrefab;
	public GameObject GameListingPrefab{
		get{ return _gameListingPrefab; }
	}

	private List<GameListing> _gameListingButtons = new List<GameListing> ();
	public List<GameListing> GameListingButtons{
		get{ return _gameListingButtons; }
	}

	public NetworkDiscovery networkDiscovery;


	public Dictionary<string,NetworkBroadcastResult> nbr;

	public void StartSearch(){
		if (GameListingButtons.Count > 0) {
			foreach (GameListing gameListing in GameListingButtons) {
				GameObject gameListingObj = gameListing.gameObject;
				GameListingButtons.Remove (gameListing);
				Destroy (gameListingObj);
			}
		}
		nbr = networkDiscovery.broadcastsReceived;
		StartCoroutine (SearchGame());
	}

	private IEnumerator SearchGame(){
		yield return new WaitForSeconds (2.0f);
		foreach (string str in nbr.Keys) {
			string ip = str.Remove (0, 7);
			GameObject gameListingObj = Instantiate (GameListingPrefab);
			gameListingObj.transform.SetParent (transform, false);

			GameListing gameListing = gameListingObj.GetComponent<GameListing> ();
			GameListingButtons.Add (gameListing);
			gameListing.SetGameIPText (ip);
		}

	}
}
