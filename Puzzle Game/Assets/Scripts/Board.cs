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

    public static Board Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allSquares = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int rows = 0; rows < width; rows++)
        {
            for (int columns = 0; columns < height; columns++)
            {
                Vector2 tempPosition = new Vector2(rows, columns);
                GameObject tile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                tile.transform.parent = this.transform;
                tile.name = "Tile" + "( " + rows + "," + columns + " )";

                int squareToUse = Random.Range(0, squares.Length);
                GameObject square = Instantiate(squares[squareToUse], tempPosition, Quaternion.identity);
                square.transform.parent = this.transform;
                square.name = "Tile" + "( " + rows + "," + columns + " )";
            }
        }
    }

}
