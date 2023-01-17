using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    private static Board instance;

    [SerializeField] private int width;
    [SerializeField] private int height;

    public GameObject tilePrefab;

    public GameObject[] squares;

    public GameObject[,] allSquares;

    public bool hasRowDecreased = false;

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
                GameObject tile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                tile.transform.parent = this.transform;
                tile.name = "Tile" + "( " + rows + "," + columns + " )";

                int squareToUse = Random.Range(0, squares.Length);
                int maxIterations = 0;

                while (MatchesAt(rows, columns, squares[squareToUse]) && maxIterations < 100)
                {
                    squareToUse = Random.Range(0, squares.Length);
                    maxIterations++;
                }

                GameObject square = Instantiate(squares[squareToUse], tempPosition, Quaternion.identity);
                square.transform.parent = tile.transform;
                square.name = square.tag + " Tile" + "( " + rows + "," + columns + " )";
                allSquares[rows, columns] = square;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (piece.CompareTag(allSquares[column - 1, row].tag) && piece.CompareTag(allSquares[column - 2, row].tag))
                return true;

            if (piece.CompareTag(allSquares[column, row - 1].tag) && piece.CompareTag(allSquares[column, row - 2].tag))
                return true;
        }

        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
                if (allSquares[column, row - 1].CompareTag(piece.tag) && allSquares[column, row - 2].CompareTag(piece.tag))
                    return true;

            if (column > 1)
                if (allSquares[column - 1, row].CompareTag(piece.tag) && allSquares[column - 2, row].CompareTag(piece.tag))
                    return true;
        }

        return false;
    }

    public void DestroyMatchesAt(int column, int row)
    {
        if (allSquares[column, row].GetComponent<Square>().isInBlock && allSquares[column, row].GetComponent<Square>().IsClicked)
        {
            Destroy(allSquares[column, row]);
            allSquares[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allSquares[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    public IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allSquares[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allSquares[i, j].GetComponent<Square>().y -= nullCount;
                    allSquares[i, j] = null;
                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);

        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allSquares[i, j] == null)
                {
                    print(i + j);
                    Vector2 tempPosition = new Vector2(i, j);
                    int squareToUse = Random.Range(0, squares.Length);
                    GameObject square = Instantiate(squares[squareToUse], tempPosition, Quaternion.identity);
                    allSquares[i, j] = square;
                }
            }
        }

        BlockFinder.Instance.HasTraversed = false;
    }

    public IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);
    }
}
