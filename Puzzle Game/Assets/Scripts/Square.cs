using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    public int x;
    public int y;

    public SQUARE_COLOR color;

    public bool visited = false;

    public bool isInBlock = false;

    [SerializeField] private Button squareButton;

    public Block parentBlock;

    private bool isClicked = false;

    private void Awake()
    {
        switch (transform.tag)
        {
            case "Green":
                color = SQUARE_COLOR.GREEN;
                break;
            case "Blue":
                color = SQUARE_COLOR.BLUE;
                break;
            case "Red":
                color = SQUARE_COLOR.RED;
                break;
            case "Magenta":
                color = SQUARE_COLOR.MAGENTA;
                break;
            case "Yellow":
                color = SQUARE_COLOR.YELLOW;
                break;
            default:
                Debug.LogError("No tag");
                break;
        }
    }
    private void Start()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        Board.Instance.allSquares[(int)transform.position.x, y] = this.gameObject;
    }

    public void OnSquareClick()
    {
        if (parentBlock != null)
        {
            parentBlock.SetClickedTrueForEachSqaure();
            Board.Instance.DestroyMatches();
        }
        else
        {
            print("no parent");
        }
        //parentblock.print();
    }

    public Block ParentBlock { get => parentBlock; set => parentBlock = value; }
    public bool IsClicked { get => isClicked; set => isClicked = value; }
}

public enum SQUARE_COLOR
{
    RED,
    GREEN,
    BLUE,
    MAGENTA,
    YELLOW,
}