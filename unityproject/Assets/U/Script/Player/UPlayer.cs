using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction이 정의됨
using UnityEngine.Events;

public class UPlayer : UCharacter, IController
{
    private static Dictionary<string, UnityAction<UPlayer, List<string>>> actionDic = new Dictionary<string, UnityAction<UPlayer, List<string>>>()
    {
        //Idle의 valList 값 State 
        { "Idle", (o, valList) => {o.ChangeState(new IdleState());  } },

        //Move의 valList 값 State/InputX/InputY 
        { "Move", (o, valList) => {o.ChangeState(new MoveState()); } }


        //이 앞으론 예시


        //Attack의 valList 값 State/InputX/InputY/데미지/경직시간/부가효과

        //피격을 받았을 때 valList 값 피격_State/데미지/경직시간/부가_디버프

        //기절 했을 때 valList 기절_State/기절시간

        //기타등등
    };

    public bool DoMove()
    {
        if (moveVector.x == 0f && moveVector.y == 0f) return false;
        else return true;
    }

    List<string> ReturnTheStateList(string _state)
    {
        List<string> action = new List<string>();

        string actionStr = _state;
        action.Add(actionStr);

        return action;
    }

    public void OrderAction(List<string> actionList)
    {
        foreach (var action in actionList)
        {
            var valList = new List<string>(action.Split('/'));
            if (actionDic.TryGetValue(valList[0], out UnityAction<UPlayer, List<string>> actionFunc))
            {
                actionFunc(this, valList);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        boxCollider2D   = GetComponent<BoxCollider2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();

        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        if(DoMove())
        {
            // 방향에 따라 캐릭터 모델 보는 방향 변경
            if (moveVector.x > 0f)      spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
            else if(moveVector.x < 0f)  spumPrefabs.transform.localScale = new Vector3(1, 1, 1);

            //ChangeState(new UMoveState());
            OrderAction(ReturnTheStateList("Move"));
        }
        else
        {
            //ChangeState(new UIdleState());
            OrderAction(ReturnTheStateList("Idle"));
        }

        base.Update();
    }
}
