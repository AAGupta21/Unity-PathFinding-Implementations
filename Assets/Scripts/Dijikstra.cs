using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijikstra
{
    List<Nodes> Map;
    Vector2Int mapsize;

    public Dijikstra(List<Nodes> temp, Vector2Int tempSize)
    {
        Map = temp;
        mapsize = tempSize;

    }

    public List<Nodes> CalcPath(Vector3 InitialPos, Vector3 Dest)
    {
        Vector2Int TempIPos = ReCalcPoint(InitialPos.x, InitialPos.z);      // converting vector3 to vector2int for easier time. Same for Destination point.

        Vector2Int TempFPos = Vector2Int.zero;
        TempFPos.x = (int)Dest.x;
        TempFPos.y = (int)Dest.z;

        ResetMapData(Map);     //Resetting all 'Checked' value in "Nodes" to false.

        Nodes StartNode = Map[TempIPos.x * mapsize.x + TempIPos.y];     //the node player is at (or close to).
        Nodes DestNode = Map[TempFPos.x * mapsize.x + TempFPos.y];      // "    "  destination is at.

        List<Nodes> Path = new List<Nodes>();

        if (StartNode != DestNode)
        {
            FindPath(StartNode, DestNode);

            Nodes temp = DestNode;

            while(temp!= StartNode)
            {
                if (temp == null)
                    Debug.Log("Problem");

                Path.Add(temp);
                temp = temp.PrevNode;
            }
        }

        Path.Add(StartNode);
        
        return Path;
    }

    void FindPath(Nodes Pnode, Nodes dest)
    {
        Pnode.FCost = 0f;

        List<Nodes> ArrayOfNodes = new List<Nodes>();

        ArrayOfNodes.Add(Pnode);
        int cnt = 0;

        while (cnt < Map.Count)
        {
            Nodes temp = ArrayOfNodes[0];

            foreach(Nodes n in temp.NeighbourList)
            {
                float TempFcost;

                if ((Mathf.Abs(n.PosX - temp.PosX) + Mathf.Abs(n.PosY - temp.PosY)) > 1f)
                    TempFcost = temp.FCost + 1.4f;
                else
                    TempFcost = temp.FCost + 1f;

                if(n.FCost == -1f)
                {
                    n.FCost = TempFcost;
                    n.PrevNode = temp;
                    ArrayOfNodes.Add(n);
                }

                if (n.FCost > TempFcost)
                {
                    n.FCost = TempFcost;
                    n.PrevNode = temp;
                }


            }

            ArrayOfNodes.Remove(temp);

            for(int i = 0; i < ArrayOfNodes.Count - 2; i++)
            {
                if(ArrayOfNodes[i].FCost > ArrayOfNodes[i+1].FCost)
                {
                    Nodes ntemp = ArrayOfNodes[i + 1];
                    ArrayOfNodes[i + 1] = ArrayOfNodes[i];
                    ArrayOfNodes[i] = ntemp;
                }
            }

            cnt++;

            if (dest.PrevNode != null)
                break;
        }
    }


    Vector2Int ReCalcPoint(float x, float y)        //Rounding off Initial Point to 0.5f and changing it to Vector2Int.
    {
        Vector2Int p = Vector2Int.zero;

        p.x = (int)x;
        p.y = (int)y;

        if (x % 1f >= 0.5f)
            p.x = (int)p.x + 1;
        else
            p.x = (int)p.x;

        if (y % 1f >= 0.5f)
            p.y = (int)p.y + 1;
        else
            p.y = (int)p.y;

        return p;
    }

    public void ResetMapData(List<Nodes> Map)
    {
        foreach (Nodes n in Map)
        {
            n.Checked = false;
            n.PrevNode = null;
            n.FCost = -1f;
        }
    }
}
