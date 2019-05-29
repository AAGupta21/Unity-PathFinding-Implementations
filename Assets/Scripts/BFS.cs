using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS
{
    List<Nodes> Map;
    Vector2Int mapsize;

    public BFS(List<Nodes> M, Vector2Int MSize)
    {
        Map = M;
        mapsize = MSize;
    }

    public List<Nodes> CalcPath(Vector3 InitialPos, Vector3 Dest)
    {
        Vector2Int TempIPos = ReCalcPoint(InitialPos.x, InitialPos.z);      // converting vector3 to vector2int for easier time. Same for Destination point.

        Vector2Int TempFPos = Vector2Int.zero;
        TempFPos.x = (int)Dest.x;
        TempFPos.y = (int)Dest.z;

        ResetMapData(Map);     //Resetting all 'Checked' value in "Nodes" to false.

        List<Nodes> Path = new List<Nodes>();
        List<Nodes> ToCheck = new List<Nodes>();
        
        Nodes StartNode = Map[TempIPos.x * mapsize.x + TempIPos.y];     //the node player is at (or close to).
        Nodes DestNode = Map[TempFPos.x * mapsize.x + TempFPos.y];      // "    "  destination is at.
        Nodes PNode = StartNode;        // The node I traverse with throughout the map.

        ToCheck.Add(PNode);
        
        int cnt = 0;
        PNode.Checked = true;

        while(cnt < Map.Count && cnt > -1)
        {
            ToCheck.Remove(PNode);

            foreach (Nodes n in PNode.NeighbourList)
            {
                if (!n.Checked)
                {
                    n.PrevNode = PNode;

                    if (n == DestNode)
                    {
                        cnt = -2;  //Found
                    }
                    ToCheck.Add(n);

                    n.Checked = true;
                }
            }
            
            if (cnt != Map.Count && ToCheck.Count > 0)
            {
                PNode = ToCheck[0];
            }

            cnt++;
        }

        if(cnt == -1)
        {
            PNode = DestNode;
            
            while(PNode != StartNode)
            {
                Path.Add(PNode);
                PNode = PNode.PrevNode;
            }

            Path.Add(PNode);
        }
        else
        {
            Debug.Log("No Direct Path Found!");
        }
        
        return Path;
    }

    Vector2Int ReCalcPoint(float x, float y)        //Rounding off Initial Point to 0.5f and changing it to Vector2Int.
    {
        Vector2Int p = Vector2Int.zero;

        p.x = (int) x;
        p.y = (int) y;


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
        foreach(Nodes n in Map)
        {
            n.Checked = false;
            n.PrevNode = null;

            if (n.type == NodeType.Blocked)
                n.Checked = true;
        }
    }
}
