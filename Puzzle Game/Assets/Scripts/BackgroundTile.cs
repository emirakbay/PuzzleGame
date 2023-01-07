using System.Collections;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] squares;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int squareIndex = Random.Range(0, squares.Length);
        GameObject square = Instantiate(squares[squareIndex], transform.position, Quaternion.identity);
        square.transform.parent = this.transform;
        square.name = this.gameObject.name;
    }
}
