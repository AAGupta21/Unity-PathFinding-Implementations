using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star
{
    List<Nodes> Map;
    Vector2Int mapsize;

    public A_Star(List<Nodes> tMap, Vector2Int tsize)
    {
        mapsize = tsize;
        Map = tMap;
    }

    public List<Nodes> CalcPath(Vector3 InitialPos, Vector3 Dest)
    {
        Debug.Log("In A_Star");

        Vector2Int TempIPos = ReCalcPoint(InitialPos.x, InitialPos.z);      // converting vector3 to vector2int for easier time. Same for Destination point.

        Vector2Int TempFPos = Vector2Int.zero;
        TempFPos.x = (int)Dest.x;
        TempFPos.y = (int)Dest.z;

        ResetMapData(Map);     //Resetting all 'Checked' value in "Nodes" to false.

        List<Nodes> Path = new List<Nodes>();

        Nodes StartNode = Map[TempIPos.x * mapsize.x + TempIPos.y];     //the node player is at (or close to).
        Nodes DestNode = Map[TempFPos.x * mapsize.x + TempFPos.y];      // "    "  destination is at.
        Nodes PNode = StartNode;        // The node I traverse with throughout the map.

        List<Nodes> OpenNodes = new List<Nodes>();      //Nodes Havent been visited.

        PNode.HCost = (GetDist(StartNode, DestNode));
        PNode.GCost = 0f;
        PNode.FCost = PNode.HCost;

        if(PNode != DestNode)
        {
            Search(PNode, DestNode, OpenNodes);
        }

        Nodes temp = DestNode;

        while (temp != StartNode)
        {
            if (temp == null)
                Debug.Log("Problem");

            Debug.Log(temp.PosX + ":" + temp.PosY);
            Path.Add(temp);
            temp = temp.PrevNode;
        }

        Path.Add(StartNode);

        return Path;
    }

    void Search(Nodes PNode, Nodes DestNode, List<Nodes> OpenNodes)
    {
        OpenNodes.Add(PNode);
        int cnt = 0;
        
        while(DestNode.PrevNode == null && cnt < Map.Count)
        {
            Nodes temp = OpenNodes[0];    //Node which have the lowest fcost, hcost.
            
            //Debug.Log("Initial Temp Node: " + temp.PosX + ":" + temp.PosY + " F: " + temp.FCost + " G: " + temp.GCost + " H: " + temp.HCost);

            foreach (Nodes l in OpenNodes)
            {
                if (l != temp && l.Checked == false)
                {
                //    Debug.Log("Node: " + l.PosX + ":" + l.PosY + " F: " + l.FCost + " G: " + l.GCost + " H: " + l.HCost);

                    if ((l.FCost < temp.FCost) || (l.FCost == temp.FCost && l.HCost < temp.HCost))
                        temp = l;
                }
            }

            temp.Checked = true;
            cnt++;
            //Debug.Log("New Temp Node: " + temp.PosX + ":" + temp.PosY + " F: " + temp.FCost + " G: " + temp.GCost + " H: " + temp.HCost);

            
            foreach (Nodes n in temp.NeighbourList)
            {
                if (n == DestNode)
                {
                    n.PrevNode = temp;
                }

                if (DestNode.PrevNode == null)
                {
                    float tempHcost = GetDist(n, DestNode);

                    float tempGcost, tempFcost;

                    if (Mathf.Abs(temp.PosX - n.PosX) + Mathf.Abs(temp.PosY - n.PosY) > 1f)
                    {
                        tempGcost = 1.4f + temp.GCost;
                    }
                    else
                    {
                        tempGcost = 1f + temp.GCost;
                    }

                    tempFcost = tempGcost + tempHcost;

                    if(n.FCost == -1f)
                    {
                        OpenNodes.Add(n);
                        n.PrevNode = temp;
                        n.FCost = tempFcost;
                        n.GCost = tempGcost;
                        n.HCost = tempHcost;
                    }

                    if ((n.FCost > tempFcost) || ((n.FCost == tempFcost) && (n.HCost > tempHcost)))
                    {
                        n.PrevNode = temp;
                        n.FCost = tempFcost;
                        n.GCost = tempGcost;
                        n.HCost = tempHcost;
                    }
                }
            }

            OpenNodes.Remove(temp);

            if (DestNode.PrevNode != null)
                break;
        }
    }
    
    float GetDist(Nodes a, Nodes b)
    {
        return Mathf.Max(Mathf.Abs(a.PosX - b.PosX), Mathf.Abs(a.PosY - b.PosY));
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
            n.PrevNode = null;
            n.FCost = -1f;
            n.GCost = -1f;
            n.HCost = -1f;
            n.Checked = false;
        }
    }
}