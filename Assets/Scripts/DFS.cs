using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS
{
    List<Nodes> Map;
    Vector2Int mapsize;

    public DFS(List<Nodes> temp, Vector2Int tempSize)
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
            Search(StartNode, DestNode);
        else
        {
            Path.Add(StartNode);
            return Path;
        }

        if (Path == null)
            Debug.Log("Send Help!");

        if(DestNode.Checked != true)
        {
            Debug.Log("No Path Found!");
            Path.Add(StartNode);
            return Path;
        }
        
        Nodes temp = DestNode;

        while (temp!= StartNode)
        {
            Debug.Log(temp.PosX + ":" + temp.PosY);
            Path.Add(temp);
            temp = temp.PrevNode;
        }

        Path.Add(StartNode);

        return Path;
    }

    void Search(Nodes PNode, Nodes DestNode)
    {
        Stack<Nodes> stack = new Stack<Nodes>();
        stack.Push(PNode);

        Nodes Ptemp;

        while(stack.Count != 0)
        {
            int cnt = 0;
            Ptemp = stack.Peek();
            
            foreach(Nodes n in Ptemp.NeighbourList)
            {
                if(n.Checked == false)
                {
                    n.PrevNode = Ptemp;
                    n.Checked = true;
                    stack.Push(n);
                    cnt++;
                    if (n == DestNode)
                    {
                        break;
                    }
                }
            }

            if (stack.Peek() == DestNode)
                break;

            if (cnt == 0)
                stack.Pop();
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
        }
    }
}