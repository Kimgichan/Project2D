using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityAction�� ���ǵ�
using UnityEngine.Events;

public class KTestMoveScript : MonoBehaviour, IController
{
    private static Dictionary<string, UnityAction<List<string>>> actionDic = new Dictionary<string, UnityAction<List<string>>>()
    {
        //Idle�� valList �� State 
        { "Idle", (valList) => { } },

        //Move�� valList �� State/InputX/InputY 
        { "Move", (valList) => { } }


        //�� ������ ����


        //Attack�� valList �� State/InputX/InputY/������/�����ð�/�ΰ�ȿ��

        //�ǰ��� �޾��� �� valList �� �ǰ�_State/������/�����ð�/�ΰ�_�����

        //���� ���� �� valList ����_State/�����ð�

        //��Ÿ���
    };


    // enum ���� namespace �������� �� Ŭ���� ������ ����־� �ּ���.
    public enum State
    {
        Idle,
        Move,
        Attack,
        Death
    }

    //�� Ȯ�θ� ������Ѽ� �ܺο��� ����Ǵ� ���� ����
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
