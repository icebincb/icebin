using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Astar
{
    public class Node : IComparable<Node>
    {
        public Vector2Int girdPosition;
        //距离起始点的距离
        public int gCost = 0;
        //距离终点的距离
        public int hCost = 0;
        public int fCost =>gCost+hCost;//当前格子的价值
        public bool isObstacale=false;
        public Node parentNode;

        public Node(Vector2Int pos)
        {
            girdPosition = pos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            int result = fCost.CompareTo(other.fCost);
            if (result == 0)
                return hCost.CompareTo(other.hCost);
            return result;
        }
    }
}