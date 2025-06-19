using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Play,
    Pause,
    Clear,
    Gameover,
    FinalResult,
    AskEnd,
    DieDeley
}

public class GameManager : MonoBehaviour
{
    [Header("Grid System")]
    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gridObjects;
    public int gridWidth = 9;
    public int gridHeight = 9;

    public static GameManager Instance;

    public UIcode UIManager;

    public GameState GS;
    public int nGameScore_current;
    public int nGameScore_Best;
    public float fGametime;
    public static int nLevel;
    public float LeftLimit = -8.9f;
    public float RightLimit = 9;
    public float TopLimit = 30f;
    public float BottomLimit = -9f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UIManager = FindObjectOfType<UIcode>();
        if (UIManager == null)
            Debug.LogError("UIManager not found.");
    }

    private void Start()
    {
        InitializeGame();
        GenerateGrid();
    }

    private void InitializeGame()
    {
        Debug.LogFormat("Loading a new level {0}...", nLevel);
        GS = GameState.Ready;
        nGameScore_current = 0;
        nGameScore_Best = 0;
        fGametime = 0f;
        Time.timeScale = 1f;

        if (nLevel >= 1)
            SoundManager.Instance.PlayBackgroundMusic(nLevel);
    }

    private void GenerateGrid()
    {
        gridObjects = new GameObject[gridWidth, gridHeight];

        float startingX = (-gridWidth / 2.0f) + 0.5f;
        float startingY = (-gridHeight / 2.0f) + 0.5f;
        float borderThickness = 0.05f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject cell = Instantiate(gridCellPrefab, new Vector3(startingX + x, startingY + y, 0), Quaternion.identity, transform);
                gridObjects[x, y] = cell;

                CreateBorder(cell.transform, borderThickness);
            }
        }
    }

    private void CreateBorder(Transform parent, float thickness)
    {
        string[] sides = { "Top", "Bottom", "Left", "Right" };
        Vector3[] scales = {
            new Vector3(1, thickness, 1), new Vector3(1, thickness, 1),
            new Vector3(thickness, 1, 1), new Vector3(thickness, 1, 1)
        };
        Vector3[] positions = {
            new Vector3(0, 0.5f - thickness / 2, 0),
            new Vector3(0, -0.5f + thickness / 2, 0),
            new Vector3(-0.5f + thickness / 2, 0, 0),
            new Vector3(0.5f - thickness / 2, 0, 0)
        };

        for (int i = 0; i < sides.Length; i++)
        {
            GameObject border = new GameObject($"{sides[i]} Border");
            border.transform.SetParent(parent);
            border.transform.localScale = scales[i];
            border.transform.localPosition = positions[i];
            border.AddComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public Vector2 GetGridBoundary()
    {
        return new Vector2(gridWidth / 2.0f, gridHeight / 2.0f);
    }

    public void ClearGame()
    {
        nLevel += 1;
        SceneManager.LoadScene(nLevel < 6 ? nLevel : 0);
    }

    public void GameOver()
    {
        SoundManager.Instance.GameOver();
        SceneManager.LoadScene(0);
    }

    public void AddScore(int scoreToAdd = 1)
    {
        nGameScore_current += scoreToAdd;
    }
}
