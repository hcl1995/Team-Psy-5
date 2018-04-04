using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FindMatchLayout : MonoBehaviour {
	float refresh = 0;
	[SerializeField]
	private GameObject _matchListingPrefab;
	public GameObject MatchListingPrefab{
		get{ return _matchListingPrefab; }
	}

	private List<MatchListing> _matchListingButtons = new List<MatchListing> ();
	public List<MatchListing> MatchListingButtons{
		get{ return _matchListingButtons; }
	}

	public NetworkDiscovery networkDiscovery;

	public Dictionary<string,NetworkBroadcastResult> nbr;

	void OnEnable(){
		StartSearch ();
	}

	public void StartSearch(){
		nbr = networkDiscovery.broadcastsReceived;
		StartCoroutine (SearchGame());
	}

	private IEnumerator SearchGame(){
		yield return new WaitForSeconds (1.5f);
		if (MatchListingButtons.Count > 0) {
			foreach (MatchListing networkListing in MatchListingButtons) {
				GameObject gameListingObj = networkListing.gameObject;
				//MatchListingButtons.Remove (networkListing);
				Destroy (gameListingObj);

			}
			MatchListingButtons.Clear();
		}
		foreach (string str in nbr.Keys) {			
			string ip = str.Remove (0, 7);
			Debug.Log (ip);
			GameObject matchListingObj = Instantiate (MatchListingPrefab);
			matchListingObj.transform.SetParent (transform, false);

			MatchListing matchListing = matchListingObj.GetComponent<MatchListing> ();
			MatchListingButtons.Add (matchListing);
			matchListing.SetGameIPText (ip);
		}

	}

	public void Cancel(){
		LobbyController.s_Singleton.CancelFromMatchFinding ();
	}
}
