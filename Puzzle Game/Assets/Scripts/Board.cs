using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{


    private static Board instance;

    [SerializeField] private int width;
    [SerializeField] private int height;

    public GameObject tilePrefab;

    public GameObject[] squares;

    private BackgroundTile[,] allTiles;

    public GameObject[,] allSquares;

    public static Board Instance { get { return instance; } }

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        allTiles = new BackgroundTile[Width, Height];
        allSquares = new GameObject[Width, Height];
        SetUp();
    }

    private void SetUp()
    {
        for (int rows = 0; rows < Width; rows++)
        {
            for (int columns = 0; columns < Height; columns++)
            {
                Vector2 tempPosition = new Vector2(rows, columns);
                GameObject tile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                tile.transform.parent = this.transform;
                tile.name = "Tile" + "( " + rows + "," + columns + " )";

                int squareToUse = Random.Range(0, squares.Length);
                GameObject square = Instantiate(squares[squareToUse], tempPosition, Quaternion.identity);
                square.transform.parent = tile.transform;
                square.name = square.tag + " Tile" + "( " + rows + "," + columns + " )";
                allSquares[rows, columns] = square;
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var arr = FindConnectedGroups(SQUARE_COLOR.RED);
            foreach (var item in arr)
            {
                foreach (var item2 in item)
                {
                    print(item2);
                }
            }
        }
    }

    public List<List<Square>> FindConnectedGroups(SQUARE_COLOR matchColor)
    {
        var groups = new List<List<Square>>();

        // Search the world grid for pairs of connected blocks.
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var block = allSquares[x, y];

                // Skip blocks we've already grouped.
                // If you don't want to add a visited field to every block, 
                // you can accomplish this with a parallel array instead.
                if (block.GetComponent<Square>().visited)
                    continue;

                // Remove this check if you want to find groups of any color.
                if (block.GetComponent<Square>().color == matchColor)
                {
                    // Every group of 2+ blocks has a block to the right or below another,
                    // so by checking just these two directions we don't exclude any.
                    if (x > 0)
                    {
                        //var neighbor = world.GetBlock(x - 1, y);
                        var neighbour = allSquares[x - 1, y];

                        if (neighbour.GetComponent<Square>().color == block.GetComponent<Square>().color)
                        {
                            var group = new List<Square>();
                            PopulateGroup(group, block.GetComponent<Square>());
                            groups.Add(group);
                            continue;
                        }

                        if (neighbour.GetComponent<Square>().color == block.GetComponent<Square>().color)
                        {
                            var group = new List<Square>();
                            PopulateGroup(group, block.GetComponent<Square>());
                            groups.Add(group);
                            continue;
                        }
                    }
                    if (y > 0)
                    {
                        var neighbour = allSquares[x, y - 1];
                        if (neighbour.GetComponent<Square>().color == block.GetComponent<Square>().color)
                        {
                            var group = new List<Square>();
                            PopulateGroup(group, block.GetComponent<Square>());
                            groups.Add(group);
                        }
                    }
                }
            }
        }

        // Set all tiles back to unvisited for the next use.
        foreach (var item in allSquares)
        {
            item.GetComponent<Square>().visited = false;
        }

        return groups;
    }

    // Recursively find connected blocks (depth-first search)
    void PopulateGroup(List<Square> group, Square block)
    {
        group.Add(block);
        block.GetComponent<Square>().visited = true;

        // Check all four neighbors and recurse on them if needed:
        if (block.GetComponent<Square>().x > 0)
        {
            var neighbor = allSquares[block.GetComponent<Square>().x - 1, block.GetComponent<Square>().y];
            if (neighbor.GetComponent<Square>().color == block.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }

        if (block.GetComponent<Square>().x < Width - 1)
        {
            var neighbor = allSquares[block.x + 1, block.y];
            if (neighbor.GetComponent<Square>().color == block.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
        if (block.y > 0)
        {
            var neighbor = allSquares[block.x, block.y - 1];
            if (neighbor.GetComponent<Square>().color == block.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
        if (block.y < Height - 1)
        {
            var neighbor = allSquares[block.x, block.y + 1];
            if (neighbor.GetComponent<Square>().color == block.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
    }
}
