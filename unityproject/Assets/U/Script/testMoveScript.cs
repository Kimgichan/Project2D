using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyState
{
    idle,
    move,
    attack,
    death,
}

public class testMoveScript : MonoBehaviour, IController
{
    private SPUM_Prefabs    spumPrefabs;
    private Rigidbody2D     playerRigidbody;

    public float            maxSpeed = 10.0f;
    public  MyState         myState;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        spumPrefabs     = GetComponent<SPUM_Prefabs>();
        
        // 시작 시 상태 idle
        myState         = MyState.idle;

    }

    // 매개변수로 들어올 값은 예("Move/0.5f/0.5f)" 3개의 문자열이 들어온다.
    public void OrderAction(params object[] orders)
    {
        // List크기 만큼 반복한다.
        for(int actionNum = 0; actionNum<orders.Length; actionNum++)
        {
            string str              = orders[actionNum] as string;
            string[] splitString    = str.Split(new char[] { '/' });
            
            // 3개의 문자열로 나누어졌는 가?
            if (splitString.Length != 3)
            {
                Debug.Log("splitString.Length길이에 문제가 있습니다.");
            }

            string myState  = splitString[0];
            float inputX    = float.Parse(splitString[1]);
            float inputY    = float.Parse(splitString[2]);

            switch(myState)
            {
                case "Idle":
                    spumPrefabs.PlayAnimation(0);
                    break;
                case "Move":
                    // inputX,inputY만큼 이동한다.
                    transform.Translate(inputX * maxSpeed * Time.deltaTime, inputY * maxSpeed * Time.deltaTime, 0f);
                    
                    // 움직이는 방향으로 프리팹(캐릭터 보는 방향)을 바꾼다.
                    if (inputX > 0)
                    {
                        spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if (inputX < 0)
                    {
                        spumPrefabs.transform.localScale = new Vector3(1, 1, 1);
                    }
                    
                    // 달리는 애니메이션 변경
                    spumPrefabs.PlayAnimation(1);
                    break;
            }
        }
    }

    // 받은 매개변수기반으로 List<string>형태를 반환하는 함수
    // 어디에 사용하는 가? -> OrderAction함수에 인자로 넘긴다.
    List<string> ReturnTheState(string _state,float _inputX, float _inputY)
    {
        List<string> action = new List<string>();

        string actionStr = _state + "/" + _inputX.ToString() + "/" + _inputY.ToString();
        action.Add(actionStr);

        return action;
    }

    // Update is called once per frame
    void Update()
    {
        //Input Manager Axes를 이용해 이동 값을 받는 다.
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        
        if(inputX == 0f && inputY == 0f)
        {
            myState = MyState.idle;
        }
        else
        {
            myState = MyState.move;
        }

        switch(myState)
        {
            case MyState.idle:
                OrderAction(ReturnTheState("Idle", inputX, inputY));
                break;
            case MyState.move:
                OrderAction(ReturnTheState("Move", inputX, inputY));
                break;
        }

    }

}
