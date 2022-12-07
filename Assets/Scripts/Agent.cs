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
    int[,] evaluated_board;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
        shop = Shop.instance;
        timer = 0.0;
        waitingTime = 1;
        evaluated_board = new int[16, 16];
        //GetBoard();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > waitingTime)
        {
            
            List<Node> node_list = GetNodes();
            
            if (PlayerStats.Money >= 100)
            {
                //build turret at (random) node
                //노드를 선택해야 함
                //BuildTower(RandomNode(node_list));
                BuildTower(BestNode());
            }
            
            Debug.Log($"Agent running!");
            timer = 0;
        }
    }

    public Node RandomNode(List<Node> node_list)
    {
        return node_list[Random.Range(0, node_list.Count)];
    }

    public Node BestNode()
    {
        int best_score = 0;
        Point best_point = new Point(-1, -1);
        int[,] current_board = GetBoard();

        for (int i=0; i < 16; i++)
        {
            for (int j=0; j<16; j++)
            {
                if (current_board[i, j] == 0 || evaluated_board[i, j] == -1)
                    continue;
                int temp = 0;
                if ( i-1 > -1 && j-1 > -1 && current_board[i-1, j-1] == 0)
                    temp++;
                if ( i-1 > -1 && current_board[i-1, j] == 0)
                    temp++;
                if ( i-1 > -1 && j+1 < 16 && current_board[i-1, j+1] == 0)
                    temp++;

                if ( j-1 > -1 && current_board[i, j-1] == 0)
                    temp++;
                if ( j+1 < 16 && current_board[i, j+1] == 0)
                    temp++;

                if ( i+1 < 16 && j-1 > -1 && current_board[i+1, j-1] == 0)
                    temp++;
                if ( i+1 < 16 && current_board[i+1, j] == 0)
                    temp++;
                if ( i+1 < 16 && j+1 < 16 && current_board[i+1, j+1] == 0)
                    temp++;
                evaluated_board[i, j] = temp;
            }
        }

        for (int i=0; i < 16; i++)
        {
            for (int j=0; j<16; j++)
            {
                if (best_score < evaluated_board[i, j])
                {
                    best_score = evaluated_board[i, j];
                    best_point.x = i;
                    best_point.y = j;
                }
            }
        }
        // 빌드할 곳을 찾은 경우 반환
        if (best_point.x != -1)
            return PointToNode(best_point);
        
        // 그렇지 못한 경우 한칸 넓혀서 탐색
        /*
            구현필요
        */
        // 다시 null인 경우 업그레이드 시도?

        for (int i=0; i < 16; i++)
        {
            for (int j=0; j<16; j++)
            {
                if (evaluated_board[i, j] == -1)
                {
                    //업그레이드 (i,j) -> 60원이니까 호출됐을 경우 실행이 가능하긴함... 따로 빼낼 것.
                    best_point.x = i;
                    best_point.y = j;
                    PointToNode(best_point).UpgradeTurret();
                    evaluated_board[i, j] = -2;
                    return null;
                }
            }
        }
        return null;
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

    public struct Point
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
            //Vector3 vec = node.transform.position;
            Point point = NodeToPoint(node);
            current_board[point.x, point.y] = 1;
            Debug.Log($"{point.x}, {point.y} = {current_board[point.x, point.y]}");
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

    //노드는 가로 x=0~75, 세로 z=0~-75까지 존재함.
    // 노드를 보드형태의 좌표로 반환
    public Point NodeToPoint(Node _node)
    {
        Point result_point;
        Vector3 node_vector = _node.transform.position;
        result_point.x = (int)(node_vector.x/5);
        result_point.y = (int)((-node_vector.z)/5);
        
        return result_point;
    }

    // 보드의 좌표를 노드로 반환
    public Node PointToNode(Point _point)
    {
        Vector3 node_vector = new Vector3(_point.x * 5, 0, _point.y * -5);
        List<Node> node_list = GetNodes();
        foreach(Node node in node_list)
        {
            Vector3 temp_vector = node.transform.position;
            if (node_vector == temp_vector)
            {
                return node;
            }
        }
        return null;
    }

    // 전달받은 노드에 특정 타워를 빌드함. 타입 없을 시 기본 타워.
    public void BuildTower(Node _node, int type = 1)
    {
        if (_node == null)
            return;
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
        Point builtat = NodeToPoint(_node);
        Debug.Log($"Build and Set Score -1 to point( {builtat.x}, {builtat.y} )");
        evaluated_board[builtat.x, builtat.y] = -1;
    }
}
