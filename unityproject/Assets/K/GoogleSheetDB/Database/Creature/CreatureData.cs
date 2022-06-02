using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여기 변수는 모두 참고용, 예시로 MinDamage보다 더 작은 데미지를 나갈 수 있고, MaxDamage보다 더 높게 데미지를 뽑을수도 있다. 그냥 기본 성능치가 이렇다고 참고하면 될 듯.
/// 
/// 좋은 참고 클래스가 구성되면 제시할 예정.
/// </summary>
[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Object/Creature", order = int.MaxValue)]
public class CreatureData : ScriptableObject
{
    [SerializeField] private int hp;

    /// <summary>
    /// 속도 배율<br/>
    /// unitSpeed * speed
    /// </summary>
    [SerializeField] private float speed;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;

    /// <summary>
    /// 밀어내는 정도의 배율<br/>
    /// unitPush * push
    /// </summary>
    [SerializeField] private float push;


    /// <summary>
    /// 공격했을 때 상대가 스턴에 걸리는 정도 1f -> 1초(s) 
    /// </summary>
    [SerializeField] private float stunTimer;

    #region 크리쳐(생물) 정보 API
    /// <summary>
    /// HP => 생물(Creature)의 MAX 체력.
    /// </summary>
    public int HP => hp;

    /// <summary>
    /// Speed => 생물의 속도, 이 속도를 참고해서 '이동(EX -> Speed*1.0f)', '대쉬(EX -> Speed*1.5f)', '공격속도(EX -> Speed*0.8f)'를 잘 조절하시면 될 듯.
    /// </summary>
    public float Speed => speed * GameManager.Instance.GameDB.UnitValueDB.UnitSpeed;



    /// <summary>
    /// MinDamage => 생물의 기본 최소 공격 기대치.
    /// </summary>
    public int MinDamage => minDamage;


    /// <summary>
    /// MaxDamage => 생물의 기본 최대 공격 기대치.
    /// </summary>
    public int MaxDamage => maxDamage;


    /// <summary>
    /// 밀쳐지는 정도
    /// </summary>
    public float PushEnergy => push * GameManager.Instance.GameDB.UnitValueDB.UnitPush;
    #endregion



#if UNITY_EDITOR
    /// <summary>
    /// 인 게임(빌드) 중에는 사용하지 말 것, 쓸 때 반드시 #if UNITY_EDITOR로 감싸줄것.
    /// 셋팅할 때 사용됨
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="speed"></param>
    /// <param name="minDamage"></param>
    /// <param name="maxDamage"></param>
    public void WriteData(int hp, float speed, int minDamage, int maxDamage, float push, float stunTimer)
    {
        Debug.LogError("경고: 인게임에서 수치를 변경하면 안 되는 값들입니다.");
        this.hp = hp;
        this.speed = speed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.push = push;
        this.stunTimer = stunTimer;
    }
#endif
}
