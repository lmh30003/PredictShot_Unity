using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainRoutine : MonoBehaviour {
    public PlayerData mainPlayer;
    public PlayerData player1, player2, player3;
    public List<PlayerData> players;
    private UIScript handUI;
    public Map map;
    const float TurnTime = 1.5f;
    const float RestTime = 10.0f;

    int Round;
    int Turn;
    int MaxTurn = 10;
    double Time;
    bool isRestTime;
    // Use this for initialization
    void Start () {
        Round = 0;
        Turn = 0;
        isRestTime = true;
        players = new List<PlayerData>(4);
        players.Add(mainPlayer);
        players.Add(player1);
        players.Add(player2);
        players.Add(player3);
        StartCoroutine(singlePlayMainRoutine());
        handUI = GameObject.Find("UIScripts").GetComponent<UIScript>();
    }
    private void Update()
    {
        //command here
        if (Input.GetKeyDown(KeyCode.C))
        {
            handUI.drawHand = !handUI.drawHand;
        }
    }
    bool isGameEnded()
    {
        int liveCount = 0;
        foreach(PlayerData player in players)
        {
            if (!player.isDead) liveCount++;
        }
        return liveCount <= 1;
    }
	void mainRoutine()
    {
        
        foreach (PlayerData player in players)
        {
            player.updateTurn();
        }

    }
    IEnumerator singlePlayMainRoutine()
    {
        while (!isGameEnded())
        {
            print(string.Format("Routine called{0}", Turn));
            if (isRestTime)
            {
                foreach (PlayerData player in players)
                {
                    player.updateRound();
                }
                isRestTime = false;
                Turn = 0;
                yield return new WaitForSeconds(RestTime);
                continue;
            }


            yield return new WaitForSeconds(TurnTime);
            foreach (PlayerData player in players)
            {
                player.updateTurn();
            }
            map.updateMap();
            Turn++;
            if(Turn == MaxTurn)
            {
                isRestTime = true;
            }
            
        }
    }
}
