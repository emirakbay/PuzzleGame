using UnityEngine;

public class Square : MonoBehaviour
{
    private Color color;

    private bool isMatch;

    private void Awake()
    {
        switch (transform.tag)
        {
            case "Green":
                color = Color.GREEN;
                break;
            case "Blue":
                color = Color.BLUE;
                break;
            case "Red":
                color = Color.RED;
                break;
            case "Magenta":
                color = Color.MAGENTA;
                break;
            case "Yellow":
                color = Color.YELLOW;
                break;
            default:
                Debug.LogError("No tag");
                break;
        }
    }

    private void Start()
    {
        print(color + " " + transform.name);
    }


    private void CheckMatch()
    {
        //if (column )
        //GameObject leftSquare = Board.Instance.allSquares[col]
    }

}

enum Color
{
    BLUE,
    GREEN,
    RED,
    MAGENTA,
    YELLOW,
}