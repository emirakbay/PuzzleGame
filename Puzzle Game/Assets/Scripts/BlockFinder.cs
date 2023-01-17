using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockFinder : MonoBehaviour
{
    private static BlockFinder instance;

    public static BlockFinder Instance { get { return instance; } }

    public bool HasTraversed { get => hasTraversed; set => hasTraversed = value; }

    public Dictionary<SQUARE_COLOR, List<List<Square>>> dict = new Dictionary<SQUARE_COLOR, List<List<Square>>>();

    List<List<Square>> redSquares = new List<List<Square>>();

    List<Block> blocks = new List<Block>();

    private bool hasTraversed = false;

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
    }

    void Update()
    {
        if (GameManager.Instance.IsGameStarted)
        {
            if (!HasTraversed)
            {
                var arr = FindConnectedGroups2();
                TraverseColorList(arr);
                HasTraversed = true;
            }
        }
    }

    public void TraverseColorList(List<List<Square>> squareBlocks)
    {
        foreach (var blocks in squareBlocks)
        {
            Block blockGroup = gameObject.AddComponent<Block>();
            blockGroup.numberOfSquares = blocks.Count;
            blockGroup.blockColor = blocks.First().color;
            foreach (var square in blocks)
            {
                square.isInBlock = true;
                square.ParentBlock = blockGroup;
                blockGroup.squareList.Add(square);
            }
        }
    }

    public List<List<Square>> FindConnectedGroups2()
    {
        var groups = new List<List<Square>>();

        // Search the board grid for pairs of connected blocks.
        for (int x = 0; x < Board.Instance.Width; x++)
        {
            for (int y = 0; y < Board.Instance.Height; y++)
            {
                var square = Board.Instance.allSquares[x, y];

                // Skip blocks we've already grouped.
                if (square.GetComponent<Square>().visited)
                    continue;

                // Every group of 2+ blocks has a block to the right or below another,
                // so by checking just these two directions we don't exclude any.
                if (x > 0)
                {
                    var neighbor = Board.Instance.allSquares[x - 1, y];
                    if (neighbor.GetComponent<Square>().color == square.GetComponent<Square>().color)
                    {
                        var group = new List<Square>();
                        PopulateGroup(group, square.GetComponent<Square>());
                        groups.Add(group);
                        continue;
                    }
                }
                if (y > 0)
                {
                    var neighbour = Board.Instance.allSquares[x, y - 1];
                    if (neighbour.GetComponent<Square>().color == square.GetComponent<Square>().color)
                    {
                        var group = new List<Square>();
                        PopulateGroup(group, square.GetComponent<Square>());
                        groups.Add(group);
                    }
                }
            }
        }

        //Set all tiles back to unvisited for the next use.
        foreach (var item in Board.Instance.allSquares)
        {
            item.GetComponent<Square>().visited = false;
        }

        return groups;
    }

    public List<List<Square>> FindConnectedGroups(SQUARE_COLOR searchColor)
    {
        var groups = new List<List<Square>>();

        // Search the board grid for pairs of connected blocks.
        for (int x = 0; x < Board.Instance.Width; x++)
        {
            for (int y = 0; y < Board.Instance.Height; y++)
            {
                var square = Board.Instance.allSquares[x, y];

                // Skip blocks we've already grouped.
                if (square.GetComponent<Square>().visited)
                    continue;

                if (square.GetComponent<Square>().color == searchColor)
                {
                    // Every group of 2+ blocks has a block to the right or below another,
                    // so by checking just these two directions we don't exclude any.
                    if (x > 0)
                    {
                        var neighbor = Board.Instance.allSquares[x - 1, y];
                        if (neighbor.GetComponent<Square>().color == square.GetComponent<Square>().color)
                        {
                            var group = new List<Square>();
                            PopulateGroup(group, square.GetComponent<Square>());
                            groups.Add(group);
                            continue;
                        }
                    }
                    if (y > 0)
                    {
                        var neighbour = Board.Instance.allSquares[x, y - 1];
                        if (neighbour.GetComponent<Square>().color == square.GetComponent<Square>().color)
                        {
                            var group = new List<Square>();
                            PopulateGroup(group, square.GetComponent<Square>());
                            groups.Add(group);
                        }
                    }
                }
            }
        }

        // Set all tiles back to unvisited for the next use.
        foreach (var item in Board.Instance.allSquares)
        {
            item.GetComponent<Square>().visited = false;
        }

        return groups;
    }

    // Recursively find connected squares (depth-first search)
    private void PopulateGroup(List<Square> group, Square square)
    {
        group.Add(square);
        square.GetComponent<Square>().visited = true;

        // Check all four neighbors and recurse on them if needed:
        if (square.GetComponent<Square>().x > 0)
        {
            var neighbor = Board.Instance.allSquares[square.GetComponent<Square>().x - 1, square.GetComponent<Square>().y];
            if (neighbor.GetComponent<Square>().color == square.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
        if (square.GetComponent<Square>().x < Board.Instance.Width - 1)
        {
            var neighbor = Board.Instance.allSquares[square.x + 1, square.y];
            if (neighbor.GetComponent<Square>().color == square.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
        if (square.y > 0)
        {
            var neighbor = Board.Instance.allSquares[square.x, square.y - 1];
            if (neighbor.GetComponent<Square>().color == square.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
        if (square.y < Board.Instance.Height - 1)
        {
            var neighbor = Board.Instance.allSquares[square.x, square.y + 1];
            if (neighbor.GetComponent<Square>().color == square.color && neighbor.GetComponent<Square>().visited == false)
                PopulateGroup(group, neighbor.GetComponent<Square>());
        }
    }

    private List<List<Square>> FindColorBlocks(SQUARE_COLOR searchColor)
    {
        return searchColor switch
        {
            SQUARE_COLOR.BLUE => FindConnectedGroups(SQUARE_COLOR.BLUE),
            SQUARE_COLOR.GREEN => FindConnectedGroups(SQUARE_COLOR.GREEN),
            SQUARE_COLOR.RED => FindConnectedGroups(SQUARE_COLOR.RED),
            SQUARE_COLOR.MAGENTA => FindConnectedGroups(SQUARE_COLOR.MAGENTA),
            SQUARE_COLOR.YELLOW => FindConnectedGroups(SQUARE_COLOR.YELLOW),
            _ => null,
        };
    }
}
