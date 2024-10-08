using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour{
    public LayerMask unwalkableMask;
    public float nodeRadius;
    public Vector2 gridWorldSize;
    public Node[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Awake(){
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        CreateGrid();
    }

    void CreateGrid() {
        grid =new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up*gridWorldSize.y/2;

        for (int x = 0; x<gridSizeX;x++){
            for (int y = 0; y<gridSizeY;y++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable =!Physics2D.OverlapCircle(worldPoint,nodeRadius,unwalkableMask);
                grid[x,y] = new Node(walkable,worldPoint,x,y);
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Horizontal and vertical neighbors
        int[] directions = { -1, 1 }; // Directions for left/right and down/up

        foreach (int x in directions)
        {
            // Check horizontal neighbors (left and right)
            int checkX = node.gridX + x;
            int checkY = node.gridY;

            if (checkX >= 0 && checkX < gridSizeX)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }

        foreach (int y in directions)
        {
            // Check vertical neighbors (down and up)
            int checkX = node.gridX;
            int checkY = node.gridY + y;

            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }


    //get player Position
    public Node NodeFromWorldPoint(Vector3 worldPostion){
        float percentX = (worldPostion.x + gridWorldSize.x/2)/gridWorldSize.x;
        float percentY = (worldPostion.y + gridWorldSize.y/2)/gridWorldSize.y;
        //value is btwn 0 and 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        //-1 to make sure the position is not outside the grid

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        return grid[x,y];
    }

    public List<Node> path;
    public Transform player;

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,gridWorldSize.y,1));
        if (grid != null){
            Node playerNode =NodeFromWorldPoint(player.position);
            foreach(Node n in grid){
                Gizmos.color = n.walkable?Color.white:Color.red;
                if (playerNode == n) { Gizmos.color = Color.cyan; }
                if(path!= null)
                    if(path.Contains(n))Gizmos.color=Color.green;
                
                Gizmos.DrawCube(n.worldPosition,  Vector3.one * (nodeDiameter-.1f));
            }
        }
    }

}