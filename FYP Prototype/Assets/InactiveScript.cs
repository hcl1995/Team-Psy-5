using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InactiveScript : MonoBehaviour {
	public bool isPlay = false;
	public float inactiveElapsed = 0.0f;
	public VideoPlayer videoPlayer;
	public GameObject menuCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inactiveElapsed += Time.deltaTime;
		if (inactiveElapsed >= 15.0f&&!isPlay) {
			videoPlayer.Play();
			isPlay = true;
			menuCanvas.SetActive (false);
		}
		if(Input.GetAxis("Mouse X")<0){
			//Code for action on mouse moving left
			StopVideo();
		}
		if(Input.GetAxis("Mouse X")>0){
			//Code for action on mouse moving right
			StopVideo();
		}
		if(Input.GetAxis("Mouse Y")<0){
			//Code for action on mouse moving left
			StopVideo();
		}
		if(Input.GetAxis("Mouse Y")>0){
			//Code for action on mouse moving right
			StopVideo();
		}
		if (Input.anyKey) {
			StopVideo ();
		}
	}

	void StopVideo(){
		videoPlayer.Stop();
		inactiveElapsed = 0;
		isPlay = false;
		menuCanvas.SetActive (true);
	}
}
