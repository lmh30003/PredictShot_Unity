using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    public string playerName;//필요한건가? 다시 생각해 볼 필요 있을듯
    public int maxHP;//const로 선언하지 않은 이유는 특정 버프로 인해 늘거나 줄어들 수도 있기 때문
    public int maxAP;
    public int HP;//현재 HP
    public int AP;//현재 AP
    public int healHP;//round당 회복 HP -> server에서만 필요할듯 하지만 싱글플레이를 위해 일단 만들어 놓자
    public int healAP;//round당 회복 AP
    public double penaltyTurn;//현재 PenaltyTurn
    public double sightAngle;//시야 범위 : 부채꼴 각도
    public int sightRange;//시야 범위 : 원의 크기라고 생각해
    public double lookingDirection;//현재 보고 있는 방향을 rad로 나타낸 값
    public double moveModifier;
    List<Card> hand;//현재 소유 카드 목록
    public int speed = 4;//한 턴에 최대 몇 칸이나 움직일 수 있는지
    public bool canMakeNoise = true;//default true. 버프 등으로 인해 false가 되면 makenoise함수는 무조건 null을 return한다
    public int defaultNoiseMake;//기본 noisemake값
    public int defaultNoiseHear;//소리듣기 기본값
    //public CharacterCommand command;//다음 턴에 실행할 command
    public Vector2 location;
    public int height;//캐릭터 키
    public GameObject playerObject;
    public bool isDead = false;
    // Use this for initialization
    void Start () {
        playerObject.transform.position = new Vector3(location.x * 0.5f, location.y * 0.5f);
	}
	public void updateTurn()
    {

    }
    public void updateRound()
    {

    }
    public struct GameCharacterState
    {
        public string characterName;//구분용 이름
        public int maxHPDiv;//각각의 변화량을 나타냄
        public int maxAPDiv;
        public int HPDiv;
        public int APDiv;
        public double penaltyTurnDiv;
        public double lookingDirectionDiv;
        public int sightRangeDiv;
        public List<string> removeCard; //빈 card structure이기만 해도 됨.
        public List<Card> addCard;
        public Vector2 locationDiv;
    }
}
