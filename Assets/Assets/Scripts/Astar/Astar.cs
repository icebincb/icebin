using System.Collections.Generic;
using Map;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Scripts.Astar
{
    public class Astar : Singleton<Astar>
    {
        private GridNodes gridNodes;
        private Node start;
        private Node target;
        private int width;
        private int height;
        private int originX;
        private int originY;

        private List<Node> openNodeList;//当前选择节点周围的节点
        private HashSet<Node> closeNodeList;//所有被选择的节点

        private bool findPath=false;

        public void BuildPath(string sceneName,Vector2Int startPos,Vector2Int endPos, Stack<MovementStep> npcMovementSteps)
        {
            findPath = false;
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //查找最短路
                if (FindShortestPath())
                {
                    UpdateNPCMovementStep(sceneName,npcMovementSteps);
                }
            }
        }
        /// <summary>
        /// 构建网格信息
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
        {

            if (GridMapManager.Instance.GetMapNodeData(sceneName, out Vector2Int girdDimensions,
                    out Vector2Int gridOrigin))
            {
                gridNodes = new GridNodes(girdDimensions.x, girdDimensions.y);
                width = girdDimensions.x;
                height = girdDimensions.y;
                originX = gridOrigin.x;
                originY = gridOrigin.y;
                openNodeList = new List<Node>();
                closeNodeList = new HashSet<Node>();
                
                start = gridNodes.GetNode(startPos.x - originX, startPos.y - originY);
                target=gridNodes.GetNode(endPos.x - originX, endPos.y - originY);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        
                        var pos = new Vector3Int(i + originX, j + originY);
                        var key = pos.x + "x" + pos.y + "y" + sceneName;
                        TileDetails tileDetails = GridMapManager.Instance.GetTileDetails(key);
                        
                        
                        if (tileDetails != null)
                        {
                            Node node = gridNodes.GetNode(i, j);
                            if (tileDetails.isNPCObstacle)
                            {
                                node.isObstacale = true;
                            }
                        }
                    }
                }
                return true;
            }

            return false;

            
        }

        private bool FindShortestPath()
        {
            //添加起点
            openNodeList.Add(start);
            while (openNodeList.Count > 0)
            {
                openNodeList.Sort();
                Node closeNode = openNodeList[0];
                openNodeList.RemoveAt(0);
                closeNodeList.Add(closeNode);
                if (closeNode == target)
                {
                    findPath = true;
                    break;
                    
                }
                EvaluateNeighbourNodes(closeNode);
                //添加周围的点
            }

            return findPath;
        }

        public void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentPos = currentNode.girdPosition;
            Node validNode;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(i==0&&j==0)
                        continue;
                    validNode = CheckValidNode(currentPos.x+i, currentPos.y+j);
                    if (validNode != null)
                    {
                        if (!openNodeList.Contains(validNode))
                        {
                            validNode.gCost = currentNode.gCost + GetNodesDistance(validNode, currentNode);
                            validNode.hCost = GetNodesDistance(validNode, target);
                            validNode.parentNode = currentNode;
                            openNodeList.Add(validNode);
                        }
                    }
                }
            }
        }

        private Node CheckValidNode(int x, int y)
        {
            if (x >= width || y >= height || x < 0 || y < 0)
            {
                return null;
                
            }

            Node validNode = gridNodes.GetNode(x, y);
            if (validNode.isObstacale || closeNodeList.Contains(validNode))
                return null;
            return validNode;
        }

        private int GetNodesDistance(Node nodeA, Node nodeB)
        {
            int x = Mathf.Abs(nodeA.girdPosition.x - nodeB.girdPosition.x);
            int y= Mathf.Abs(nodeA.girdPosition.y- nodeB.girdPosition.y);
            if (x > y)
            {
                return 14 * y + 10 * (x - y);
            }
            else return  14 * x + 10 * (y - x);
        }

        private void UpdateNPCMovementStep(string sceneName, Stack<MovementStep> npcMovementSteps)
        {
            Node nextNode = target;
            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();
                newStep.sceneName = sceneName;
                newStep.gridPos = new Vector2Int(nextNode.girdPosition.x + originX, nextNode.girdPosition.y + originY);
                npcMovementSteps.Push(newStep);
                nextNode = nextNode.parentNode;
            }
            
        }
    }
}