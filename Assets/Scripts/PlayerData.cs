using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    //for game
    public string playerName;//필요한건가? 다시 생각해 볼 필요 있을듯
    public int maxHP;//const로 선언하지 않은 이유는 특정 버프로 인해 늘거나 줄어들 수도 있기 때문
    public int maxAP;
    public int HP;//현재 HP
    public int AP;//현재 AP
    public int healHP;//round당 회복 HP -> server에서만 필요할듯 하지만 싱글플레이를 위해 일단 만들어 놓자
    public int healAP;//round당 회복 AP
    public float penaltyTurn;//현재 PenaltyTurn
    public float sightAngle;//시야 범위 : 부채꼴 각도
    public int sightRange;//시야 범위 : 원의 크기라고 생각해
    public float lookingDirection;//현재 보고 있는 방향을 rad로 나타낸 값. -pi < dir <= pi
    public float moveModifier;//??
    public List<Card> hand;//현재 소유 카드 목록
    public int speed = 4;//한 턴에 최대 몇 칸이나 움직일 수 있는지
    public bool canMakeNoise = true;//default true. 버프 등으로 인해 false가 되면 makenoise함수는 무조건 null을 return한다
    public int defaultNoiseMake;//기본 noisemake값
    public int defaultNoiseHear;//소리듣기 기본값
    public PlayerState command;
    //public CharacterCommand command;//다음 턴에 실행할 command
    public Vector2 location;
    public int height;//캐릭터 키
    public bool isDead = false;

    //for unity
    private Light sightLight;
    // Use this for initialization
    void Start () {
        gameObject.transform.position = new Vector3(location.x, location.y);
        sightLight = gameObject.GetComponent<Light>();
        sightLight.range = sightRange;
        sightLight.spotAngle = Mathf.Rad2Deg * sightAngle;
	}
	public void updateTurn()
    {
        //location += new Vector2(1.0f, 1.0f);
        updateState(command);


        gameObject.transform.position = new Vector3(location.x, location.y);
    }
    public void updateRound()
    {

    }
    private void updateState(PlayerState state)
    {
        if (state == null) return;
        maxHP += state.maxHPDiv;
        maxAP += state.maxAPDiv;
        HP += state.HPDiv;
        AP += state.APDiv;
        penaltyTurn += state.penaltyTurnDiv;
        lookingDirection += state.lookingDirectionDiv;
        //set looking direction value between -PI ~ PI
        if (lookingDirection > Mathf.PI) lookingDirection -= 2 * Mathf.PI;
        else if (lookingDirection <= -Mathf.PI) lookingDirection += 2 * Mathf.PI;
        if (state.removeCard != null)
        {
            foreach (string name in state.removeCard)
            {
                hand.RemoveAt(hand.FindIndex(card => card.cardName == name));
            }
        }
        if (state.addCard != null)
        {
            hand.AddRange(state.addCard);
        }
        location += state.locationDiv;
    }
    public class PlayerState
    {
        public string characterName;//구분용 이름
        public int maxHPDiv;//각각의 변화량을 나타냄
        public int maxAPDiv;
        public int HPDiv;
        public int APDiv;
        public float penaltyTurnDiv;
        public float lookingDirectionDiv;
        public int sightRangeDiv;
        public List<string> removeCard; //빈 card structure이기만 해도 됨.
        public List<Card> addCard;
        public Vector2 locationDiv;
    }
}
