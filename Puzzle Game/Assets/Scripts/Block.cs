using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int numberOfSquares;
    public SQUARE_COLOR blockColor;
    public List<Square> squareList = new List<Square>();

    private void Start()
    {
        //Instantiate(gameObject);
    }


    public void SetClickedTrueForEachSqaure()
    {
        foreach (Square item in squareList)
        {
            item.IsClicked = true;
            Destroy(this);
            //BlockFinder.Instance.HasTraversed = false;
        }
    }

}
