using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction이 정의됨
using UnityEngine.Events;

public class KTestMoveScript : MonoBehaviour, IController
{
    private static Dictionary<string, UnityAction<List<string>>> actionDic = new Dictionary<string, UnityAction<List<string>>>()
    {
        //Idle의 valList 값 State 
        { "Idle", (valList) => { } },

        //Move의 valList 값 State/InputX/InputY 
        { "Move", (valList) => { } }


        //이 앞으론 예시


        //Attack의 valList 값 State/InputX/InputY/데미지/경직시간/부가효과

        //피격을 받았을 때 valList 값 피격_State/데미지/경직시간/부가_디버프

        //기절 했을 때 valList 기절_State/기절시간

        //기타등등
    };


    // enum 들은 namespace 개념으로 각 클래스 안으로 집어넣어 주세요.
    public enum State
    {
        Idle,
        Move,
        Attack,
        Death
    }

    //값 확인만 노출시켜서 외부에서 변경되는 것을 막음
    public State state {get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    public void OrderAction(List<string> actionList)
    {
        foreach(var action in actionList)
        {
            var valList = new List<string>(action.Split('/'));
            if(actionDic.TryGetValue(valList[0], out UnityAction<List<string>> actionFunc))
            {
                actionFunc(valList);
            }
        }
    }
}
