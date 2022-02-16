using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AStar : MonoBehaviour
{

    //������������������������������������������������������������������������������������������������//
    //                                         ����                                           
    // C++�� MAP�� ���� ����� �Ѵ�. ���� ��尡 ��� ��忡�� �Դ� �� �����ϴ� �뵵�� ��� 
    public Dictionary<Vector2Int, Vector2Int> parentVector = new Dictionary<Vector2Int, Vector2Int>();

    // ������ ������ ���� �� X,Y
    public int              mapSizeX = 8;
    public int              mapSizeY = 8;
    private const int       DIRECTION_SIZE = 8;

    // player�� monster�� Transfrom������ �����´�.
    public Transform        player;
    public Transform        monster;
    // startPos�� ���� ��ġ, exitPos�� �÷��̾ �� ��ġ�� �ȴ�.
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
        // 'g'�� ���� ��ġ���� ���� ��ġ �Ÿ� ���� �� �̴�.
        public int g;
        // 'h'�� ���� ��ġ���� ���� ��� ��ġ�� ���� ���̴�.
        public int h;
        // ���� ��忡 ���� x,y���� ������.
        public Vector2Int Pos;
    }

    private int[] cost =
    {
        // ���� ���⿡ ���� ��
        10,
        10,
        10,
        10,
        // �밢�� ���⿡ ���� ��
        14,
        14,
        14,
        14,
    };

    private Vector2Int[] directionPos =
    {
       new Vector2Int(0,1),     //  ��
       new Vector2Int(1,0),     //  ��
       new Vector2Int(0,-1),    // ��
       new Vector2Int(-1,0),    // ��

       new Vector2Int(1,1),     //  ��
       new Vector2Int(1,-1),    //  ��
       new Vector2Int(-1,-1),   //  ��
       new Vector2Int(-1,1),    //  ��
    };

    void Start()
    {
        // �� ��ġ ����
        scanWall();
         
        // ������ǥ�� ������ǥ�� �޾� �´�.
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

            // ������ǥ�� ������ǥ�� �޾� �´�.
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

        // ���� ���� ������ ���� ����
        int loop = 0;

        // openList�� ����� �� ���� �ݺ�
        while (openList.Count > 0)
        {
            if (++loop > 100) break;

            // �����ϸ� while�� ����
            if (myNode.Pos == exitPos) break;

            // ���� ���� 'f'���� ã�� myNode�� �����Ѵ�.
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


            // �湮ó���� ���� �� ����Ʈ�� Remove�� Add�� �Ѵ�.
            openList.Remove(myNode);
            closedList.Add(myNode.Pos);

            for (int n = 0; n < directionPos.Length; n++)
            {
                Node nextNode = myNode;

                nextNode.Pos += directionPos[n];

                // �� �� �ִ� ����ΰ�? üũ
                if (CanGo(myNode, nextNode, closedList, n)) 
                {
                    // ���¸���Ʈ�� ���� ���⿡ ���� ���� �ִ� ��.
                    nextNode.g = myNode.g + cost[n];
                    nextNode.h = (Mathf.Abs(exitPos.x - nextNode.Pos.x) + Mathf.Abs(exitPos.y - nextNode.Pos.y)) * 10;
                    nextNode.f = nextNode.g + nextNode.h;

                    openList.Add(nextNode);

                    // 'Ű' �ߺ� Ȯ���� ���ִ� �Լ� ContainsKey
                    if (!parentVector.ContainsKey(nextNode.Pos))
                        parentVector.Add(nextNode.Pos, myNode.Pos);
                }


            }
        }

        // ���� ��ǥ�� �������� ��� �Ʒ��� �����Ѵ�.
        // (�׷��� ���� ��쿡�� �ϴ� �ƹ� �ൿ�� ���ϰ� ó��)
        if (myNode.Pos == exitPos)
        {
            Vector2Int _pos = exitPos;

            lastList = new List<Vector2Int>();

            // ���� ���� ����
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
    
    // mapSize X,Y ���� �� ��ŭ ������ �ִ� ���� ����
    // ������ ���� ���� ��ġ ���� wallVectorList�� �����Ѵ�.
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

        // ���� �÷��̾� ��ġ�� ������ ������ ��ġ�� �����Ѱ�?
        if (playerPos == exitPos)
            return true;
        else
            return false;
    }

    // �÷��̾� -> ������ǥ, ���� -> ������ǥ
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
        // ���� �ִ� �� �ΰ�?
        if (wallVectorList.Contains(_nextNode.Pos)) return false;
        // �湮�� �� �ΰ�?
        if (_closedList.Contains(_nextNode.Pos)) return false;

        // �밢�� ���⿡ ���� �����ϴ� ��?
        Vector2Int checkXWall = new Vector2Int(_myNode.Pos.x + directionPos[_direction].x, _myNode.Pos.y);
        Vector2Int checkYWall = new Vector2Int(_myNode.Pos.x, _myNode.Pos.y + directionPos[_direction].y);
        if (wallVectorList.Contains(checkXWall) || wallVectorList.Contains(checkYWall)) return false;

        return true;
    }
}
