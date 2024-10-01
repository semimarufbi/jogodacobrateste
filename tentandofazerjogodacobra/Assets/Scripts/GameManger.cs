using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int[,] grid;
    public int width = 20;
    public int height = 20;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeGrid()
    {
        grid = new int[width, height];
    }

    public void SetGameDimensions(int newWidth, int newHeight)
    {
        width = newWidth;
        height = newHeight;
        InitializeGrid();
    }
}