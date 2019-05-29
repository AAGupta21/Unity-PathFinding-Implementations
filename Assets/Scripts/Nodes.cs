using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;

public enum NodeType
{
    Blocked = 0, UnBlocked = 1
}

public class Nodes
{
    public NodeType type = NodeType.UnBlocked;

    public float PosX = -1;
    public float PosY = -1;

    public float HCost = -1f;
    public float GCost = -1f;
    public float FCost = -1f;

    public Nodes(float X, float Y)
    {
        PosX = X;
        PosY = Y;
    }

    public void SetNodeType(NodeType typ)
    {
        type = typ;
    }

    public bool Checked = false;

    public List<Nodes> NeighbourList = new List<Nodes>();

    public Nodes PrevNode = null;
}