using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class UIAdaptor : MonoBehaviour {
	public RectTransform rectTrans;
	UIElement uiEle;
	// Use this for initialization
	void Start () {
		uiEle = new UIElement(rectTrans);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
