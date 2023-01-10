using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{


    private static Board instance;

    [SerializeField] private int width;
    [SerializeField] private int height;

    public GameObject tilePrefab;

    public GameObject[] squares;

    //private BackgroundTile[,] allTiles;

    public GameObject[,] allSquares;

    public static Board Instance { get { return instance; } }

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }


    public Dictionary<SQUARE_COLOR, List<List<Square>>> dict = new Dictionary<SQUARE_COLOR, List<List<Square>>>();

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

        //allTiles = new BackgroundTile[Width, Height];
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
            List<List<Square>> redSquares = FindRedSquareBlocks();
            dict.Add(SQUARE_COLOR.RED, redSquares);

            List<List<Square>> blueSquares = FindBlueSquareBlocks();
            dict.Add(SQUARE_COLOR.BLUE, blueSquares);

            List<List<Square>> greenSquares = FindGreenSquareBlocks();
            dict.Add(SQUARE_COLOR.GREEN, greenSquares);

            List<List<Square>> yellowSquares = FindYellowSquareBlocks();
            dict.Add(SQUARE_COLOR.YELLOW, yellowSquares);

            List<List<Square>> magentaSquares = FindMagentaSquareBlocks();
            dict.Add(SQUARE_COLOR.MAGENTA, magentaSquares);

            var red = dict[SQUARE_COLOR.RED];
            PrintSquareBlockList(red);

            var green = dict[SQUARE_COLOR.GREEN];
            PrintSquareBlockList(green);

            var blue = dict[SQUARE_COLOR.BLUE];
            PrintSquareBlockList(blue);

            var magenta = dict[SQUARE_COLOR.MAGENTA];
            PrintSquareBlockList(magenta);

            var yellow = dict[SQUARE_COLOR.YELLOW];
            PrintSquareBlockList(yellow);
        }
    }

    private void PrintSquareBlockList(List<List<Square>> squareBlocks)
    {
        foreach (var blocks in squareBlocks)
        {
            foreach (var square in blocks)
            {
                SpriteRenderer mySprite = square.GetComponent<SpriteRenderer>();
                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, .05f);
            }
        }
    }


    public List<List<Square>> FindConnectedGroups(SQUARE_COLOR matchColor)
    {
        var groups = new List<List<Square>>();

        // Search the board grid for pairs of connected blocks.
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

    private List<List<Square>> FindRedSquareBlocks()
    {
        return FindConnectedGroups(SQUARE_COLOR.RED);
    }

    private List<List<Square>> FindBlueSquareBlocks()
    {
        return FindConnectedGroups(SQUARE_COLOR.BLUE);
    }

    private List<List<Square>> FindGreenSquareBlocks()
    {
        return FindConnectedGroups(SQUARE_COLOR.GREEN);
    }

    private List<List<Square>> FindYellowSquareBlocks()
    {
        return FindConnectedGroups(SQUARE_COLOR.YELLOW);
    }

    private List<List<Square>> FindMagentaSquareBlocks()
    {
        return FindConnectedGroups(SQUARE_COLOR.MAGENTA);
    }
}
