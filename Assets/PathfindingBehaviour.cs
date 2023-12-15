using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingBehaviour : MonoBehaviour
{
    public GameObject collider;
    CompositeCollider2D obstacles;
    public int xMin = -27;
    int xMax = 34;
    public int yMin = -20;
    int yMax = 12;

    int xstep;
    int ystep;

    public bool[,] amn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initialize(){
        xstep = xMax - xMin;
        ystep = yMax - yMin;
        amn = new bool[xstep,ystep];
        obstacles = collider.GetComponent<CompositeCollider2D>();
        for(int i = 0; i < xstep; i++){
            for(int j= 0; j <ystep; j++){
                amn[i,j] = !obstacles.OverlapPoint(new Vector2(xMin+i+0.5f, yMin+j+0.5f));
            }
        }
    }

    public List<Cell> shortestPath(bool[,] matrix, Vector2 start, Vector2 end) {
        int sx = (int)start.x - xMin, sy = (int)start.y- yMin;
        int dx = (int)end.x- xMin, dy = (int)end.y - yMin;

        //if start or end value is 0, return
        if (!matrix[sx, sy] || !matrix[dx, dy]) {
            return new List<Cell>();
        }

        //initialize the cells
        int m = matrix.GetLength(0);
        int n = matrix.GetLength(1);
        Cell[,] cells = new Cell[m, n];
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (matrix[i, j] ) {
                    cells[i, j] = new Cell(i, j, 2147483647, null);
                }
            }
        }
        //breadth first search
        Queue<Cell> queue = new Queue<Cell>();
        Cell src = cells[sx, sy];
        src.dist = 0;
        queue.Enqueue(src);
        Cell dest = null;
        Cell p;
        while ((p = queue.Dequeue()) != null) {
            //find destination
            if (p.x == dx && p.y == dy) {
                dest = p;
                break;
            }
            // moving up
            visit(cells, ref queue, p.x - 1, p.y, p);
            // moving down
            visit(cells, ref queue, p.x + 1, p.y, p);
            // moving left
            visit(cells, ref queue, p.x, p.y - 1, p);
            //moving right
            visit(cells, ref queue, p.x, p.y + 1, p);
        }

        //compose the path if path exists
        if (dest == null) {
            return new List<Cell>();
        } else {
            List<Cell> ppath = new List<Cell>();
            p = dest;
            do {
                ppath.Add(p);
            } while ((p = p.prev) != null);
            ppath.Reverse();
            return ppath;
        }
    }



    //function to update cell visiting status, Time O(1), Space O(1)
    private static void visit(Cell[,] cells, ref Queue<Cell> queue, int x, int y, Cell parent) {
        //out of boundary
        if (x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1) || cells[x, y] == null) {
            return;
        }
        //update distance, and previous node
        int dist = parent.dist + 1;
        Cell p = cells[x, y];
        if (dist < p.dist) {
            p.dist = dist;
            p.prev = parent;
            queue.Enqueue(p);
        }
    }
}

public class Cell {
    public int x;
    public int y;
    public int dist; //distance
    public Cell prev; //parent cell in the path
    public Cell(int x_, int y_, int dist_, Cell prev_) {
        x = x_;
        y = y_;
        dist = dist_;
        prev = prev_;
    }
}