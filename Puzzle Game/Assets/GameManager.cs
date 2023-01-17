using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

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

    private void Start()
    {
        IsGameStarted = true;
    }

    private bool isGameStarted;

    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }
}
