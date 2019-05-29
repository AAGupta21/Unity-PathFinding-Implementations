using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private GameObject BlockPrefab = null;
    [SerializeField] private GameObject VConnectorPrefab = null;
    [SerializeField] private GameObject HConnectorPrefab = null;

    [SerializeField] private Vector2Int MapSize = new Vector2Int();
    
    public List<Nodes> NodeData = new List<Nodes>();

    public bool MapReady = false;

    public void Start()
    {
        BuiltMap();
    }

    private void BuiltMap()
    {
        for(int i = 0; i < MapSize.x; i++)          // creates Mapsize.x * Mapsize.y nodes and add them to NodeData list.
        {
            for(int j = 0; j < MapSize.y; j++)
            {
                Nodes node = new Nodes(i, j);
                NodeData.Add(node);
            }
        }

        foreach(Nodes n in NodeData)
        {
            if(n.PosX > 0f)
            {
                n.NeighbourList.Add(NodeData[((int)n.PosX - 1) * MapSize.x + (int)n.PosY]);
            }
            
            if(n.PosX < MapSize.x - 1)
            {
                n.NeighbourList.Add(NodeData[((int)n.PosX + 1) * MapSize.x + (int)n.PosY]);
            }

            if(n.PosY > 0f)
            {
                n.NeighbourList.Add(NodeData[(int)n.PosX * MapSize.x + (int)n.PosY - 1]);
            }

            if(n.PosY < MapSize.y - 1)
            {
                n.NeighbourList.Add(NodeData[(int)n.PosX * MapSize.x + (int)n.PosY + 1]);
            }

            if(n.PosX > 0f && n.PosY > 0f)
            {
                n.NeighbourList.Add(NodeData[((int)n.PosX - 1) * MapSize.x + (int)n.PosY - 1]);
            }

            if (n.PosX > 0f && n.PosY < (MapSize.y - 1))
            {
                n.NeighbourList.Add(NodeData[((int)n.PosX - 1) * MapSize.x + (int)n.PosY + 1]);
            }

            if (n.PosX < (MapSize.x - 1) && n.PosY > 0f)
            {
                n.NeighbourList.Add(NodeData[((int)n.PosX + 1) * MapSize.x + (int)n.PosY - 1]);
            }

            if (n.PosX < (MapSize.x - 1) && n.PosY < (MapSize.y - 1))
            {
                n.NeighbourList.Add(NodeData[((int)n.PosX + 1) * MapSize.x + (int)n.PosY + 1]);
            }
        }

        for (int i = 1; i < MapSize.x - 1; i++)         //Randomly selects a node to become blocked. if a neighbour contains a block, chance goes down.
        {
            for (int j = 1; j < MapSize.y - 1; j++)
            {
                int count = 0;
                foreach (Nodes n in NodeData[i * MapSize.x + j].NeighbourList)
                {
                    if (n.type == NodeType.Blocked)
                        count++;
                }

                float Chance = Random.Range(0f, 10f);

                if ((count == 0 && Chance < 6f) || (count == 1 && Chance < 4f) || (count == 2 && Chance < 2f) )
                {
                    NodeData[i * MapSize.x + j].SetNodeType(NodeType.Blocked);
                    SpawnBlock(i, j);
                }
            }
        }
        
        MapReady = true;
        Debug.Log("Map Built");
        BuiltConnection();
    }

    void SpawnBlock(int PosX, int PosY)
    {
        Instantiate(BlockPrefab, BlockPrefab.transform.position + new Vector3(PosX, 0f, PosY), Quaternion.identity, transform);
    }

    void BuiltConnection()      // spawns a connector between 2 points, indicating the difference.
    {
        for(int i = 0; i < MapSize.x - 1; i++)
        {
            for(int j = 0; j < MapSize.y - 1; j++)
            {
                Instantiate(HConnectorPrefab, HConnectorPrefab.transform.position + new Vector3(i + 0.5f, 0f, j), HConnectorPrefab.transform.rotation, transform);
                Instantiate(VConnectorPrefab, VConnectorPrefab.transform.position + new Vector3(i, 0f, j + 0.5f), VConnectorPrefab.transform.rotation, transform);
            }
        }
        
        for (int i = 0; i < MapSize.x - 1; i++)
        {
            Instantiate(HConnectorPrefab, HConnectorPrefab.transform.position + new Vector3(i + 0.5f, 0f, MapSize.y - 1), HConnectorPrefab.transform.rotation, transform);
        }

        for (int i = 0; i < MapSize.y - 1; i++)
        {
            Instantiate(VConnectorPrefab, VConnectorPrefab.transform.position + new Vector3(MapSize.x - 1, 0f, i + 0.5f), VConnectorPrefab.transform.rotation, transform);
        }
    }

    public List<Nodes> RetMap()
    {
        return NodeData;
    }

    public Vector2Int RetMapSize()
    {
        return MapSize;
    }
}