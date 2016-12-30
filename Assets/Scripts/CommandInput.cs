using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInput : MonoBehaviour {
    public GameObject effect, movePointEffect;
    public bool drawOnProgress;
    private GameObject moveRange;
    private PlayerData playerData;
    private PlayerData.PlayerState command;
    private bool moveMode, dirChangeMode;
    Vector3 inactiveLoc = new Vector3(-5000, 0, 0);
    
    // Use this for initialization
	void Start () {
        Color col = effect.GetComponent<SpriteRenderer>().color;
        col.a = 0.5f;
        drawOnProgress = false;
        moveMode = false;
        playerData = gameObject.GetComponent<PlayerData>();
        moveRange = Instantiate(effect, gameObject.transform.position, Quaternion.identity);
        moveRange.GetComponent<SpriteRenderer>().color = col;
    }
	
	// Update is called once per frame
	void Update () {
        if (!drawOnProgress)
        {
            //now it can draw to get command
            if (Input.GetMouseButton(1))
            {
                //move command
                if (!moveMode)
                {
                    moveRange.transform.localScale *= playerData.speed * 2;
                    moveMode = true;
                }
                moveRange.transform.position = playerData.location;
                
                Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(Vector2.Distance(playerData.location, p) > playerData.speed)
                {
                    Vector2 diff = p - playerData.location;
                    float angle = Mathf.Atan2(diff.y, diff.x);
                    print(Mathf.Rad2Deg * angle);
                    p = new Vector2(playerData.location.x + Mathf.Cos(angle)*playerData.speed, playerData.location.y + Mathf.Sin(angle)*playerData.speed);
                }
                movePointEffect.transform.position = p;
            }
            else if (Input.GetMouseButton(0))
            {
                //change looking direction

            }
            else
            {
                if (moveMode)
                {
                    moveMode = false;
                    moveRange.transform.localScale /= playerData.speed * 2;
                    moveRange.transform.position = inactiveLoc;
                    
                }
                
                
            }

        }
	}
}
