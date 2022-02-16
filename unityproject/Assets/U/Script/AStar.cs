using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AStar : MonoBehaviour
{

    //￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣//
    //                                         변수                                           
    // C++에 MAP과 같은 기능을 한다. 현재 노드가 어디 노드에서 왔는 지 저장하는 용도로 사용 
    public Dictionary<Vector2Int, Vector2Int> parentVector = new Dictionary<Vector2Int, Vector2Int>();

    // 감지할 범위의 대한 값 X,Y
    public int              mapSizeX = 8;
    public int              mapSizeY = 8;
    private const int       DIRECTION_SIZE = 8;

    // player와 monster의 Transfrom정보를 가져온다.
    public Transform        player;
    public Transform        monster;
    // startPos는 몬스터 위치, exitPos는 플레이어가 될 위치가 된다.
    private Vector2Int      startPos;
    private Vector2Int      exitPos;

    public List<Vector2>    wallVectorList;
    public List<Vector2Int> lastList;
            Collider2D[]    col;
    //_______________________________________________________________________________________//


    struct Node
    {
        public Node(int _f,int _g,int _h, Vector2Int _pos)
        {
            f = _f;
            g = _g;
            h = _h;
            Pos = _pos;
        }

        // f = g+h
        public int f; 
        // 'g'는 시작 위치에서 현재 위치 거리 대한 값 이다.
        public int g;
        // 'h'는 도착 위치에서 현재 노드 위치에 대한 값이다.
        public int h;
        // 현재 노드에 대한 x,y값을 가진다.
        public Vector2Int Pos;
    }

    private int[] cost =
    {
        // 직선 방향에 대한 값
        10,
        10,
        10,
        10,
        // 대각선 방향에 대한 값
        14,
        14,
        14,
        14,
    };

    private Vector2Int[] directionPos =
    {
       new Vector2Int(0,1),     //  ↑
       new Vector2Int(1,0),     //  →
       new Vector2Int(0,-1),    // ↓
       new Vector2Int(-1,0),    // ←

       new Vector2Int(1,1),     //  ↗
       new Vector2Int(1,-1),    //  ↘
       new Vector2Int(-1,-1),   //  ↙
       new Vector2Int(-1,1),    //  ↖
    };

    void Start()
    {
        // 벽 위치 감지
        scanWall();
         
        // 시작좌표와 도착좌표를 받아 온다.
        startAndExitPos();

        AstarAlgorithm();
    }


    int test = 0;

    private void Update()
    {

        if(playerPosCheck())
        {

            Debug.Log(test);
            if (test < lastList.Count)
            {
                Vector3 testVector = new Vector3(lastList[test].x, lastList[test].y, 0);

                transform.position = Vector3.MoveTowards(transform.transform.position, testVector, 1.0f * Time.deltaTime);

                if (transform.position.x == lastList[test].x && transform.position.y == lastList[test].y)
                {
                    ++test;
                }
            }

        }
        else
        {
            test = 0;
            parentVector.Clear();
            lastList.Clear();

            // 시작좌표와 도착좌표를 받아 온다.
            startAndExitPos();

            AstarAlgorithm();

            if (test < lastList.Count)
            {
                Vector3 testVector = new Vector3(lastList[test].x, lastList[test].y, 0);

                transform.position = Vector3.MoveTowards(transform.transform.position, testVector, 1.0f * Time.deltaTime);

                if (transform.position.x == lastList[test].x && transform.position.y == lastList[test].y)
                {
                    ++test;
                }
            }

        }
    }

    void AstarAlgorithm()
    {
        int g = 0;
        int h = (Mathf.Abs(exitPos.x - startPos.x) + Mathf.Abs(exitPos.y - startPos.y)) * 10;

        Node startNode = new Node(g + h, g, h, startPos);
        Node myNode = startNode;

        List<Node> openList = new List<Node>() { startNode };
        List<Vector2Int> closedList = new List<Vector2Int>();

        parentVector.Add(startPos, startPos);

        // 무한 루프 방지를 위한 변수
        int loop = 0;

        // openList가 비어질 때 까지 반복
        while (openList.Count > 0)
        {
            if (++loop > 100) break;

            // 도착하면 while문 종료
            if (myNode.Pos == exitPos) break;

            // 가장 낮은 'f'값을 찾아 myNode를 변경한다.
            for (int i = 0; i < openList.Count; i++)
            {
                if (i + 1 == openList.Count)
                {
                    myNode = openList[i];
                    break;
                }

                if (openList[i].f <= openList[i + 1].f && openList[i].h < openList[i + 1].h)
                    myNode = openList[i];
                else
                    myNode = openList[i + 1];
            }


            // 방문처리를 위해 각 리스트를 Remove와 Add를 한다.
            openList.Remove(myNode);
            closedList.Add(myNode.Pos);

            for (int n = 0; n < directionPos.Length; n++)
            {
                Node nextNode = myNode;

                nextNode.Pos += directionPos[n];

                // 갈 수 있는 경로인가? 체크
                if (CanGo(myNode, nextNode, closedList, n)) 
                {
                    // 오픈리스트에 다음 방향에 대한 값을 넣는 다.
                    nextNode.g = myNode.g + cost[n];
                    nextNode.h = (Mathf.Abs(exitPos.x - nextNode.Pos.x) + Mathf.Abs(exitPos.y - nextNode.Pos.y)) * 10;
                    nextNode.f = nextNode.g + nextNode.h;

                    openList.Add(nextNode);

                    // '키' 중복 확인을 해주는 함수 ContainsKey
                    if (!parentVector.ContainsKey(nextNode.Pos))
                        parentVector.Add(nextNode.Pos, myNode.Pos);
                }


            }
        }

        // 도착 좌표에 도달했을 경우 아래를 진행한다.
        // (그렇지 못한 경우에는 일단 아무 행동도 못하게 처리)
        if (myNode.Pos == exitPos)
        {
            Vector2Int _pos = exitPos;

            lastList = new List<Vector2Int>();

            // 무한 루프 방지
            int loopTwo = 0;

            while (true)
            {
                if (++loopTwo > 100) break;

                lastList.Add(_pos);

                if (_pos == parentVector[_pos]) break;

                _pos = parentVector[_pos]; ;
            }

            lastList.Reverse();
        }
    }
    
    // mapSize X,Y 변수 값 만큼 범위에 있는 벽을 감지
    // 감지된 벽에 대한 위치 값은 wallVectorList에 저장한다.
    void scanWall()
    {
        for (int y = 0; y < mapSizeY; y++)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                col = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.4f);

                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(x, y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        Vector2 wallVector = new Vector2(x, y);
                        wallVectorList.Add(wallVector);
                    }
            }
        }
    }

    bool playerPosCheck()
    {
        Vector2Int playerPos = new Vector2Int();
        playerPos.x = (int)player.position.x;
        playerPos.y = (int)player.position.y;

        // 현재 플레이어 위치와 이전에 감지한 위치와 동일한가?
        if (playerPos == exitPos)
            return true;
        else
            return false;
    }

    // 플레이어 -> 도착좌표, 몬스터 -> 시작좌표
    void startAndExitPos()
    {
        //startPos.x  = (int)monster.position.x;
        //startPos.y  = (int)monster.position.y;

        startPos.x = (int)transform.position.x;
        startPos.y = (int)transform.position.y;

        exitPos.x   = (int)player.position.x;
        exitPos.y   = (int)player.position.y;
        Debug.Log(exitPos);
    }

    bool CanGo(Node _myNode,Node _nextNode,List<Vector2Int> _closedList, int _direction )
    {
        // 벽이 있는 곳 인가?
        if (wallVectorList.Contains(_nextNode.Pos)) return false;
        // 방문한 곳 인가?
        if (_closedList.Contains(_nextNode.Pos)) return false;

        // 대각선 방향에 벽이 방해하는 가?
        Vector2Int checkXWall = new Vector2Int(_myNode.Pos.x + directionPos[_direction].x, _myNode.Pos.y);
        Vector2Int checkYWall = new Vector2Int(_myNode.Pos.x, _myNode.Pos.y + directionPos[_direction].y);
        if (wallVectorList.Contains(checkXWall) || wallVectorList.Contains(checkYWall)) return false;

        return true;
    }
}
