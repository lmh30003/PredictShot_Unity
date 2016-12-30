using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {
    private Texture2D handBackground, statusBackground, commandBackground;
    private GUIStyle backgroundGUI;
    public Color bgColor;
    public bool drawHand;
    public bool drawStatus;
    private PlayerData player;
	// Use this for initialization
	void Start () {
        handBackground = new Texture2D(Screen.width, Screen.height / 4);
        bgColor.a = 0.3f;
        for(int x = 0; x < handBackground.width; x++)
        {
            for(int y = 0; y < handBackground.height; y++)
            {
                handBackground.SetPixel(x, y, bgColor);
            }
        }
        handBackground.Apply();
        backgroundGUI = new GUIStyle();
        backgroundGUI.normal.background = handBackground;
        player = GameObject.Find("MainPlayer").GetComponent<PlayerData>();
	}
    void OnGUI()
    {
        if (drawHand)
        {
            GUI.Box(new Rect(0, Screen.height * 3 / 4, handBackground.width, handBackground.height), GUIContent.none, backgroundGUI);
        }
        
    }
    void Update()
    {
        if (drawHand)
        {
            //draw hand algorithm here
            foreach(Card c in player.hand)
            {

            }
        }
    }
}
