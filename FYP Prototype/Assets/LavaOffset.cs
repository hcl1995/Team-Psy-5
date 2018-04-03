using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaOffset : MonoBehaviour {

	public float scrollSpeed = 0.5F;
	public Material mat;
	float offset;
	void Start(){
		mat.SetTextureOffset("_MainTex", new Vector2(0, 0));
	}
	void Update() {
		offset += Time.deltaTime * scrollSpeed;
		mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
	}
}
