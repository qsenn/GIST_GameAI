using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Agent : MonoBehaviour
{
    BuildManager buildManager;
    Shop shop;
    double timer;
    int waitingTime;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
        shop = Shop.instance;
        timer = 0.0;
        waitingTime = 5;
        //GetBoard();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > waitingTime)
        {
            
            List<Node> node_list = GetNodes();
            //int[,] current_board = GetBoard();
            
            if (PlayerStats.Money >= 100)
            {
                //build turret at (random) node
                //노드를 선택해야 함
                RandomBuild(node_list);
            }
            GetBoard();
            
            Debug.Log($"Agent running\nNode List Count: {node_list.Count}");
            timer = 0;
        }
    }

    public void RandomBuild(List<Node> node_list)
    {
        BuildTower(node_list[Random.Range(0, node_list.Count)]);
    }

    public List<Node> GetNodes()
    {
        List<Node> node_list = new List<Node>();
        GameObject[] node_array = GameObject.FindGameObjectsWithTag("NODE");
        for (int i=0; i<node_array.Length; i++)
        {
            if (node_array[i] != null)
            {
                node_list.Add(node_array[i].GetComponent<Node>());
            }
        }
        return node_list;
    }

    struct Point
    {
        public int x, y;
        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    // 노드의 상태(건물 업으면 0, 있으면 1)를 16*16 2차원 배열로 반환
    public int[,] GetBoard()
    {
        int[,] current_board = new int[16, 16];
        List<Node> node_list = GetNodes();
        foreach(Node node in node_list)
        {
            //Vector3 vec = node.GetBuildPosition();
            Vector3 vec = node.transform.position;
            Debug.Log($"{vec.x}, {vec.y}, {vec.z}");
            current_board[(int)(vec.x/5), (int)((-vec.z)/5)] = 1;
        }
        /*
        // 보드 확인용 출력
        Debug.Log($"--------board--------------");
        string temp = "";
        for (int i=0; i < 16; i++)
        {
            for (int j=0; j<16; j++)
            {
                temp = temp + " " + current_board[j, i].ToString();
            }
            temp = temp + "\n";
        }
        Debug.Log($"{temp}");
        Debug.Log($"---------------------------");
        */
        return current_board;
    } 
/*
    //노드는 가로 x=0~75, 세로 z=0~-75까지 존재함.
    // 노드를 보드형태의 좌표로 반환
    public Point NodeToPoint(Node _node)
    {
        Point result_point;
        
        return result_point;
    }

    // 보드의 좌표를 노드로 반환
    public Node PointToNode(int x, int y)
    {
        Node result_node;
        
        return result_node;
    }
*/
    // 전달받은 노드에 특정 타워를 빌드함. 타입 없을 시 기본 타워.
    public void BuildTower(Node _node, int type = 1)
    {
        if (_node.turret != null)
			return;
        switch (type)
        {
            case 1:
                _node.BuildTurret(shop.standardTurret);
                break;
            case 2:
                _node.BuildTurret(shop.missileLauncher);
                break;
            case 3:
                _node.BuildTurret(shop.laserBeamer);
                break;
        }
    }
}
