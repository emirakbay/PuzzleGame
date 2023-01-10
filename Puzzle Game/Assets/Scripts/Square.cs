using UnityEngine;

public class Square : MonoBehaviour
{

    public int x;
    public int y;

    public SQUARE_COLOR color;

    public bool visited = false;

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