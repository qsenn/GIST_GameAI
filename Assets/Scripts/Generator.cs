using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public Transform spawnPoint;
    public GameObject waypoint;
    public Waypoints waypoints;
    public GameObject endpoint;
    public GameObject node;
    public GameObject path;
    public List<Vector3> paths;
    public List<Vector3> forbiddens;

    public static List<GameObject> node_list;
    public static int node_count;
    public static List<GameObject> path_list;
    public static int path_count;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {   
        node_list = new List<GameObject>();
        path_list = new List<GameObject>();
        Vector3 startPos;
        Vector3 lastWaypoint;
        startPos = spawnPoint.transform.position;
        startPos.y = 0;
        lastWaypoint = startPos;
        int numOfWaypoints = Random.Range(5,20);
        int numOfCorners = Random.Range(20,20);
        SetWaypoints2(numOfCorners, lastWaypoint);
        GetPath();
        PutTiles();
        
    }

    public List<GameObject> GetNodeList()
    {
        return node_list;
    }

    public List<GameObject> GetPathList()
    {
        return path_list;
    }

    public bool checkEnd(){
        return true;
    }

    public void SetWaypoints(int numOfWaypoints, Vector3 lastWaypoint){
        int flag = Random.Range(0, 2);
        int rand = 0;

        GameObject startPoint = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
        startPoint.transform.parent = GameObject.Find("Waypoints").transform;

        for (int i = 0; i < numOfWaypoints; i++){
            rand = Random.Range(0, 16);
            if (Mathf.Abs(rand - lastWaypoint.x / 5) <= 1.1 || Mathf.Abs(rand + lastWaypoint.z / 5) <= 1.1){
                i--;
                continue;
            }
            if (flag == 0){
                lastWaypoint.x = rand * 5;
                GameObject child = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
                child.transform.parent = GameObject.Find("Waypoints").transform;
                Debug.Log(lastWaypoint);
                flag = 1;
            }
            else if (flag == 1){
                lastWaypoint.z = rand * -5;
                GameObject child = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
                child.transform.parent = GameObject.Find("Waypoints").transform;
                Debug.Log(lastWaypoint);
                flag = 0;
            }
        }
        /* Vector3 endPoint = lastWaypoint;
        endPoint.y += (float)2.5;
        Instantiate(endpoint, endPoint, new Quaternion(0,0,0,0));
        waypoints.GetWaypoints_RD(); */
        

    }

    public void GetPath(){
        
        paths = new List<Vector3>(){};
        for (int i = 0; i < Waypoints.points.Length - 1; i++)
		{
            if(Waypoints.points[i + 1].transform.position.x > Waypoints.points[i].transform.position.x){
                for (int j = (int)Waypoints.points[i].transform.position.x; j <= (int)Waypoints.points[i + 1].transform.position.x; j+=5){
                    Vector3 _path = new Vector3(j, 0, (int)Waypoints.points[i].transform.position.z);
                    paths.Add(_path);
                }
            }
            else if(Waypoints.points[i + 1].transform.position.x < Waypoints.points[i].transform.position.x){
                for (int j = (int)Waypoints.points[i].transform.position.x; j >= (int)Waypoints.points[i + 1].transform.position.x; j-=5){
                    Vector3 _path = new Vector3(j, 0, (int)Waypoints.points[i].transform.position.z);
                    paths.Add(_path);
                }
            }
            else if(Waypoints.points[i + 1].transform.position.z > Waypoints.points[i].transform.position.z){
                for (int j = (int)Waypoints.points[i].transform.position.z; j <= (int)Waypoints.points[i + 1].transform.position.z; j+=5){
                    Vector3 _path = new Vector3((int)Waypoints.points[i].transform.position.x, 0, j);
                    paths.Add(_path);
                }
            }
            else if(Waypoints.points[i + 1].transform.position.z < Waypoints.points[i].transform.position.z){
                for (int j = (int)Waypoints.points[i].transform.position.z; j >= (int)Waypoints.points[i + 1].transform.position.z; j-=5){
                    Vector3 _path = new Vector3((int)Waypoints.points[i].transform.position.x, 0, j);
                    paths.Add(_path);
                }
            }
		}
        
    }

    public void PutTiles(){
        node_count = 0;
        for (int i = 0; i <= 15; i ++){
            for (int j = 0; j >= -15; j--){
                if (paths.Contains(new Vector3(i * 5, 0, j * 5))){
                    GameObject path_ = Instantiate(path, new Vector3(i * 5, 0, j * 5), new Quaternion(0,0,0,0)) as GameObject;
                    path_.transform.parent = GameObject.Find("Environment").transform;
                    //path_list.Add(path_);
                }
                else{
                    GameObject node_ = Instantiate(node, new Vector3(i * 5, 0, j * 5), new Quaternion(0,0,0,0)) as GameObject;
                    node_.transform.parent = GameObject.Find("Nodes").transform;
                    node_.tag = "NODE";
                    //node_list.Add(node_);
                }
            }
        }
    }



    public void SetWaypoints2(int numOfCorners, Vector3 lastWaypoint){
        int flag = Random.Range(0, 4);
        int waypoints_num = 0;
        List<int> isBlocked = new List<int>{0, 0, 0, 0};
        forbiddens = new List<Vector3>(){};

        GameObject startPoint = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
        startPoint.transform.parent = GameObject.Find("Waypoints").transform;

        bool exit = false;
        for (int i = 0; i< numOfCorners; i++){
            if (flag == 0){
                if((int)lastWaypoint.x >= 55){
                    i--;
                    flag = Random.Range(0,4);
                    continue;
                }
                waypoints_num = Random.Range(5, 15 - ((int)lastWaypoint.x / 5));
                Debug.Log(waypoints_num);
                for (int j = 0; j<waypoints_num; j++){
                    lastWaypoint.x += 5;
                    if(forbiddens.Contains(lastWaypoint)){
                        isBlocked[0] = 1;
                        lastWaypoint.x -= 5;
                        if (!isBlocked.Contains(0)){
                            
                            Debug.Log("No Way!");
                            exit = true;
                            break;
                        }
                        
                        flag = Random.Range(2,4);
                        break;
                    }
                    GameObject child = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
                    child.transform.parent = GameObject.Find("Waypoints").transform;
                    forbiddens.Add(new Vector3(lastWaypoint.x - 10, 0, lastWaypoint.z));
                    forbiddens.Add(new Vector3(lastWaypoint.x - 5, 0, lastWaypoint.z + 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x - 5, 0, lastWaypoint.z - 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x, 0, lastWaypoint.z));
                    Debug.Log(lastWaypoint);
                    flag = Random.Range(2,4);
                    isBlocked = new List<int>{0, 0, 0, 0};
                }
                if(exit == true){
                    break;
                }
                
            }
            else if (flag == 1){
                if((int)lastWaypoint.x <= 20){
                    i--;
                    flag = Random.Range(0,4);
                    continue;
                }
                waypoints_num = Random.Range(5, ((int)lastWaypoint.x / 5));
                Debug.Log(waypoints_num);
                for (int j = 0; j<waypoints_num; j++){
                    lastWaypoint.x -= 5;
                    if(forbiddens.Contains(lastWaypoint)){
                        isBlocked[1] = 1;
                        lastWaypoint.x += 5;
                        if (!isBlocked.Contains(0)){
                            
                            Debug.Log("No Way!");
                            exit = true;
                            break;
                        }
                        flag = Random.Range(2,4);
                        break;
                    }
                    GameObject child = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
                    child.transform.parent = GameObject.Find("Waypoints").transform;
                    forbiddens.Add(new Vector3(lastWaypoint.x + 10, 0, lastWaypoint.z));
                    forbiddens.Add(new Vector3(lastWaypoint.x + 5, 0, lastWaypoint.z + 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x + 5, 0, lastWaypoint.z - 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x, 0, lastWaypoint.z));
                    Debug.Log(lastWaypoint);
                    flag = Random.Range(2,4);
                    isBlocked = new List<int>{0, 0, 0, 0};
                }
                if(exit == true){
                    break;
                }
            }
            else if (flag == 2){
                if((int)lastWaypoint.z <= -55){
                    i--;
                    flag = Random.Range(0,4);
                    continue;
                }
                waypoints_num = Random.Range(5, 15 + ((int)lastWaypoint.z / 5));
                Debug.Log(waypoints_num);
                for (int j = 0; j<waypoints_num; j++){
                    lastWaypoint.z -= 5;
                    if(forbiddens.Contains(lastWaypoint)){
                        lastWaypoint.z += 5;
                        isBlocked[2] = 1;
                        if (!isBlocked.Contains(0)){
                            
                            Debug.Log("No Way!");
                            exit = true;
                            break;
                        }
                        
                        flag = Random.Range(0,2);
                        break;
                    }
                    GameObject child = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
                    child.transform.parent = GameObject.Find("Waypoints").transform;
                    forbiddens.Add(new Vector3(lastWaypoint.x, 0, lastWaypoint.z + 10));
                    forbiddens.Add(new Vector3(lastWaypoint.x - 5, 0, lastWaypoint.z + 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x + 5, 0, lastWaypoint.z + 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x, 0, lastWaypoint.z));
                    Debug.Log(lastWaypoint);
                    flag = Random.Range(0,2);
                    isBlocked = new List<int>{0, 0, 0, 0};
                }
                if(exit == true){
                    break;
                }
            }
            else if (flag == 3){
                if((int)lastWaypoint.z >= -20){
                    i--;
                    flag = Random.Range(0,4);
                    continue;
                }
                waypoints_num = Random.Range(5, -1 * ((int)lastWaypoint.z / 5));
                Debug.Log(waypoints_num);
                for (int j = 0; j<waypoints_num; j++){
                    lastWaypoint.z += 5;
                    if(forbiddens.Contains(lastWaypoint)){
                        lastWaypoint.z -= 5;
                        isBlocked[3] = 1;
                        if (!isBlocked.Contains(0)){
                            
                            Debug.Log("No Way!");
                            exit = true;
                            break;
                        }
                        
                        flag = Random.Range(0,2);
                        break;
                    }
                    GameObject child = Instantiate(waypoint, lastWaypoint, new Quaternion(0,0,0,0)) as GameObject;
                    child.transform.parent = GameObject.Find("Waypoints").transform;
                    forbiddens.Add(new Vector3(lastWaypoint.x, 0, lastWaypoint.z - 10));
                    forbiddens.Add(new Vector3(lastWaypoint.x - 5, 0, lastWaypoint.z - 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x + 5, 0, lastWaypoint.z - 5));
                    forbiddens.Add(new Vector3(lastWaypoint.x, 0, lastWaypoint.z));
                    Debug.Log(lastWaypoint);
                    flag = Random.Range(0,2);
                    isBlocked = new List<int>{0, 0, 0, 0};
                }
                if(exit == true){
                    break;
                }
            }
            
        }

        Vector3 endPoint = lastWaypoint;
        endPoint.y += (float)2.5;
        Instantiate(endpoint, endPoint, new Quaternion(0,0,0,0));
        waypoints.GetWaypoints_RD();
        

    }

}
