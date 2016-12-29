using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileBehavior : MonoBehaviour {
    public bool isSelected; // true if this tile is selected
    public bool isInsideActionRange;//true if this tile can be selected by specific action

    //public bool 
    public SpriteRenderer myTileRenderer;
    // Use this for initialization
	
    void Start () {
		
	}
	
	// Update is called once per frame
	void updateColor () {
        
        if (isSelected)
        {

        }
        if (isInsideActionRange)
        {
            myTileRenderer.color = new Color(1f, 0.4f, 1f, 0.7f);
        }
	}
}
