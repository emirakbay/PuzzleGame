using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    [SerializeField] public int x;
    [SerializeField] public int y;

    public SQUARE_COLOR color;

    public bool visited = false;

    [SerializeField] private int leftNeighbourCount;
    [SerializeField] private int rightNeighbourCount;
    [SerializeField] private int upNeighbourCount;
    [SerializeField] private int downNeighbourCount;

    private bool isMatch;

    //private bool isNeighbourColorSame;

    public List<GameObject> adjacents = new List<GameObject>();

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

        //CheckMatch();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    CheckMatch();
        //if (isMatch)
        //{
        //    SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        //    mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, .05f);
        //}
    }

    public void CheckMatch()
    {
        ////horizontal 3 match only from center
        //if (x > 0 && x < Board.Instance.Width - 1)
        //{
        //    GameObject leftSquare = Board.Instance.allSquares[x - 1, y];
        //    GameObject rightSquare = Board.Instance.allSquares[x + 1, y];
        //    if (this.gameObject.CompareTag(leftSquare.tag) && this.gameObject.CompareTag(rightSquare.tag))
        //    {
        //        print(this.gameObject.name);
        //        leftSquare.GetComponent<Square>().isMatch = true;
        //        rightSquare.GetComponent<Square>().isMatch = true;
        //        isMatch = true;
        //    }
        //}

        //// vertical 3 match only from center
        //if (y > 0 && y < Board.Instance.Height - 1)
        //{
        //    GameObject upSquare = Board.Instance.allSquares[x, y + 1];
        //    GameObject downSquare = Board.Instance.allSquares[x, y - 1];
        //    if (this.gameObject.CompareTag(upSquare.tag) && this.gameObject.CompareTag(downSquare.tag))
        //    {
        //        upSquare.GetComponent<Square>().isMatch = true;
        //        downSquare.GetComponent<Square>().isMatch = true;
        //        isMatch = true;
        //    }
        //}
    }

    //private bool IsNeighbourSameColor(int x, int y, DIRECTION dir)
    //{
    //    switch (dir)
    //    {
    //        case DIRECTION.UP:
    //            return Board.Instance.allSquares[x + 1, y].CompareTag(this.tag);
    //        case DIRECTION.DOWN:
    //            return Board.Instance.allSquares[x + 1, y].CompareTag(this.tag);
    //        case DIRECTION.LEFT:
    //            return Board.Instance.allSquares[x + 1, y].CompareTag(this.tag);
    //        case DIRECTION.RIGHT:
    //            return Board.Instance.allSquares[x + 1, y].CompareTag(this.tag);
    //        default:
    //            return false;
    //    }
    //}

    //private bool IsLeftNeighbourSameColor(int x, int y)
    //{
    //    return Board.Instance.allSquares[row - 1, column - 1].CompareTag(this.tag);
    //}

    //private bool IsTopNeighbourSameColor(int row, int column)
    //{
    //    return Board.Instance.allSquares[row + 1, column].CompareTag(this.tag);
    //}

    //private bool IsDownNeighbourSameColor(int row, int column)
    //{
    //    return Board.Instance.allSquares[row - 1, column].CompareTag(this.tag);
    //}
}

public enum SQUARE_COLOR
{
    BLUE,
    GREEN,
    RED,
    MAGENTA,
    YELLOW,
}

enum DIRECTION
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
}