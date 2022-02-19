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
        
        // ���� �� ���� idle
        myState         = MyState.idle;

    }

    // �Ű������� ���� ���� ��("Move/0.5f/0.5f)" 3���� ���ڿ��� ���´�.
    public void OrderAction(params object[] orders)
    {
        // Listũ�� ��ŭ �ݺ��Ѵ�.
        for(int actionNum = 0; actionNum<orders.Length; actionNum++)
        {
            string str              = orders[actionNum] as string;
            string[] splitString    = str.Split(new char[] { '/' });
            
            // 3���� ���ڿ��� ���������� ��?
            if (splitString.Length != 3)
            {
                Debug.Log("splitString.Length���̿� ������ �ֽ��ϴ�.");
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
                    // inputX,inputY��ŭ �̵��Ѵ�.
                    transform.Translate(inputX * maxSpeed * Time.deltaTime, inputY * maxSpeed * Time.deltaTime, 0f);
                    
                    // �����̴� �������� ������(ĳ���� ���� ����)�� �ٲ۴�.
                    if (inputX > 0)
                    {
                        spumPrefabs.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if (inputX < 0)
                    {
                        spumPrefabs.transform.localScale = new Vector3(1, 1, 1);
                    }
                    
                    // �޸��� �ִϸ��̼� ����
                    spumPrefabs.PlayAnimation(1);
                    break;
            }
        }
    }

    // ���� �Ű������������ List<string>���¸� ��ȯ�ϴ� �Լ�
    // ��� ����ϴ� ��? -> OrderAction�Լ��� ���ڷ� �ѱ��.
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
        //Input Manager Axes�� �̿��� �̵� ���� �޴� ��.
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
