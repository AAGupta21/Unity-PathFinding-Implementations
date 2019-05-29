using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Algo { BFS, DFS, A_Star, Dijikstra }

public class PlayerMovement : MonoBehaviour
{
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Vector3 TargetPoint = Vector3.zero;
    [SerializeField]private MapData Map = null;   //In inspector I have linked this to mapdata object. (Not 100% sure if it worked)
    [SerializeField] Algo algo = Algo.BFS;
    [SerializeField] float MoveSpeed = 1f;
    
    private BFS BFSObject;
    private A_Star ASObject;
    private DFS DFSObject;
    private Dijikstra djkObject;

    private List<Nodes> Path = null;
    private bool Initalized = false;
    private int cnt = -1;

    void Initialize()
    {
        Initalized = true;
        ResetNeighbourList();
        switch (algo)           //when i add future algorithms.
        {
            case Algo.BFS:
                BFSObject = new BFS(Map.RetMap(), Map.RetMapSize());
                break;
            case Algo.A_Star:
                ASObject = new A_Star(Map.RetMap(), Map.RetMapSize());
                break;
            case Algo.DFS:
                DFSObject = new DFS(Map.RetMap(), Map.RetMapSize());
                break;
            case Algo.Dijikstra:
                djkObject = new Dijikstra(Map.RetMap(), Map.RetMapSize());
                break;
        }
    }
    
    private void Update()
    {
        if(Map.MapReady)
        {
            if (!Initalized)
                Initialize();

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    CalcPoint(hit.point);
                }
            }

            if(cnt != -1)
            {
                if(Vector3.Distance(transform.position, new Vector3(Path[cnt].PosX, 0f, Path[cnt].PosY)) < 0.1f)
                {
                    cnt--;
                }
                else
                {
                    Vector3 Direc = (new Vector3(Path[cnt].PosX, 0f, Path[cnt].PosY) - transform.position).normalized;

                    transform.position += Direc * MoveSpeed * Time.deltaTime;
                }
            }
        }
        
    }

    void CalcPoint(Vector3 point)
    {
        if(Map.RetMapSize().x >= point.x && Map.RetMapSize().y >= point.z && point.x >= 0 && point.z >= 0)      // Rounds off floating point to their nearest 0.5 multiplier.
        {
            if (point.x % 1 >= 0.5f)
                point.x = (int)point.x + 1;
            else
                point.x = (int)point.x;

            if (point.z % 1 >= 0.5f)
                point.z = (int)point.z + 1;
            else
                point.z = (int)point.z;

            CalcPath(point);
        }
        else
        {
            Debug.Log("Out of Bounds");
        }
    }

    void CalcPath(Vector3 point)
    {
        switch (algo)
        {
            case Algo.BFS:
                Path = BFSObject.CalcPath(transform.position, point);
                break;

            case Algo.A_Star:
                Path = ASObject.CalcPath(transform.position, point);
                break;

            case Algo.DFS:
                Path = DFSObject.CalcPath(transform.position, point);
                break;

            case Algo.Dijikstra:
                Path = djkObject.CalcPath(transform.position, point);
                break;
        }

        foreach(Nodes n in Path)
        {
            Debug.Log("Path: " + n.PosX + ":" + n.PosY);
        }
        cnt = Path.Count - 1;
    }

    void ResetNeighbourList()
    {
        for(int i = Map.RetMap().Count - 1; i >= 0; i--)
        {
            for(int j = Map.RetMap()[i].NeighbourList.Count - 1; j >= 0; j--)
            {
                if (Map.RetMap()[i].NeighbourList[j].type == NodeType.Blocked)
                    Map.RetMap()[i].NeighbourList.Remove(Map.RetMap()[i].NeighbourList[j]);
            }
        }
    }
}