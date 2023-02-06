using UnityEngine;

namespace Assets.Scripts.Astar
{
    public class GridNodes : MonoBehaviour
    {
        public int wight;
        public int hight;

        public Node[,] gridNodes;
        /// <summary>
        /// 初始化地图节点数据
        /// </summary>
        /// <param name="wight"></param>
        /// <param name="hight"></param>
        public GridNodes(int wight, int hight)
        {
            this.wight = wight;
            this.hight = hight;

            gridNodes = new Node[wight, hight];
            for (int i = 0; i < wight; i++)
            {
                for (int j = 0; j < hight; j++)
                {
                    gridNodes[i, j] = new Node(new Vector2Int(i, j));
                }
            }
        }

        public Node GetNode(int x, int y)
        {
            if (x < wight && y < hight)
            {
                return gridNodes[x, y];
            }
            Debug.LogError("超出网格范围");
            return null;
        }
    }
}