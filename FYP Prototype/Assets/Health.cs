using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Health : NetworkBehaviour {

	public int maxHealth = 100;
	public bool destroyOnDeath;
	public Text HealthTxtMY;
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = 100;
	public HealthOwner healthBar1;
	public HealthOwner healthBar2;
	public RectTransform healthBarOwn;
	private NetworkStartPosition[] spawnPoints;
	public NetworkStartPosition mySpawn;
	public SpawnPoint[] spawnPointOwner;
	public HealthOwner[] healthOwner;
	public Health[] healths;
	public Health enemyHealth;
	public HealthCanvas hc;
	[SyncVar(hook="OnWin")]
	public int winCount;

	void Start ()
	{
		HealthTxtMY.text = currentHealth.ToString();
		if (isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
		healthOwner = FindObjectsOfType<HealthOwner> ();
		spawnPointOwner = FindObjectsOfType<SpawnPoint> ();
		healths = FindObjectsOfType<Health> ();
		foreach (HealthOwner ho in healthOwner) {
			if (ho.Owner == null) {
				ho.Owner = gameObject;
				healthBarOwn = ho.GetComponent<RectTransform> ();
				break;
			}
		}
		foreach (SpawnPoint sp in spawnPointOwner) {
			if (sp.owner == null) {
				sp.owner = gameObject;
				mySpawn = sp.GetComponent<NetworkStartPosition> ();
				break;
			}
		}
		foreach (Health h in healths) {
			if (h != this) {
				enemyHealth = h;
				break;
			}
		}
		winCount = 0;
		healthBarOwn.transform.GetComponentInChildren<Text> ().text = winCount.ToString();
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			winCount++;
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			} 
			else
			{
				//RpcCheckState ();
				currentHealth = maxHealth;
				//RpcCheckState();
				// called on the Server, invoked on the Clients
				RpcRespawn();
			}
		}
	}

	void OnChangeHealth (int currentHealth )
	{
		HealthTxtMY.text = currentHealth.ToString();
		healthBarOwn.sizeDelta = new Vector2(currentHealth, healthBarOwn.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;

			// If there is a spawn point array and the array is not empty, pick one at random
			spawnPoint = mySpawn.transform.position;
			// Set the player’s position to the chosen spawn point
			transform.position = spawnPoint;
		}
	}

	[ClientRpc]
	public void RpcRespawnGame(){
		if (isLocalPlayer)
		{
			currentHealth = maxHealth;
			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;

			// If there is a spawn point array and the array is not empty, pick one at random
			spawnPoint = mySpawn.transform.position;

			// Set the player’s position to the chosen spawn point
			transform.position = spawnPoint;
		}
	}


	public void RpcCheckState(){
		if (isLocalPlayer) {
			if (currentHealth > 0) {
				winCount++;
				healthBarOwn.transform.GetComponentInChildren<Text> ().text = winCount.ToString ();
			}
			//RpcRespawnGame ();
		}
	}
	void OnWin(int winCount){
		//RpcRespawnGame ();
		healthBarOwn.transform.GetComponentInChildren<Text> ().text = winCount.ToString ();
	}
}