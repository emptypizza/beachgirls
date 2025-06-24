using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 전체 게임 진행을 관리하는 싱글톤 매니저.
/// Flat-Top 헥사곤 타일 그리드를 중앙 정렬로 생성합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Enums & Constants

    public enum GameState
    {
        Ready,
        Play,
        Pause,
        Clear,
        Gameover,
        FinalResult,
        AskEnd,
        DieDelay    // 오타 수정
    }

    private const float DEFAULT_CELL_WIDTH = 1f;
    #endregion

    #region Inspector Fields

    [Header("Grid System")]
    [SerializeField, Tooltip("생성할 헥사곤 그리드 Prefab")]
    private GameObject gridCellPrefab;

    [SerializeField, Tooltip("그리드 가로(열) 개수"), Min(1)]
    private int gridWidth = 9;

    [SerializeField, Tooltip("그리드 세로(행) 개수"), Min(1)]
    private int gridHeight = 9;
    #endregion

    #region Public Properties

    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }
    public float GameTime { get; private set; }
    public static int Level { get; private set; } = 0;
    #endregion

    #region Private Fields

    private GameObject[,] gridObjects;
    private UIcode uiManager;

    // 월드 경계 체크용(필요 시 활용)
    private float leftLimit = -8.9f;
    private float rightLimit = 9f;
    private float topLimit = 30f;
    private float bottomLimit = -9f;
    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 씬 간 유지 필요 시 활성화
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // UI 매니저 참조
        uiManager = FindObjectOfType<UIcode>();
        if (uiManager == null)
            Debug.LogError("[GameManager] UIcode 컴포넌트를 찾을 수 없습니다.");
    }

    private void Start()
    {
        InitializeGame();
        GenerateGrid();
    }
    #endregion

    #region Initialization

    /// <summary>
    /// 게임 상태 및 점수, 시간 초기화
    /// </summary>
    private void InitializeGame()
    {
        Debug.LogFormat("=== Loading Level {0} ===", Level);
        CurrentState = GameState.Ready;
        CurrentScore = 0;
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        GameTime = 0f;
        Time.timeScale = 1f;

        // 배경음악 재생
        if (Level >= 1 && SoundManager.Instance != null)
            SoundManager.Instance.PlayBackgroundMusic(Level);
    }
    #endregion

    #region Grid Generation

    /// <summary>
    /// Flat-Top 헥사곤 그리드를 중앙 정렬하여 생성합니다.
    /// </summary>
    private void GenerateGrid()
    {
        if (gridCellPrefab == null)
        {
            Debug.LogError("[GameManager] gridCellPrefab이 할당되지 않았습니다.");
            return;
        }

        gridObjects = new GameObject[gridWidth, gridHeight];

        float cellWidth = DEFAULT_CELL_WIDTH;
        float cellHeight = Mathf.Sqrt(3f) / 2f * cellWidth;

        // 중앙 정렬 오프셋 계산
        float offsetX = -cellWidth * 0.75f * (gridWidth - 1) / 2f;
        float offsetY = -cellHeight * (gridHeight - 1) / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float posX = cellWidth * 0.75f * x + offsetX;
                float posY = cellHeight * (y + 0.5f * (x % 2)) + offsetY;

                Vector3 pos = new Vector3(posX, posY, 0);
                GameObject cell = Instantiate(gridCellPrefab, pos, Quaternion.identity, transform);
                cell.tag = "GridCell";    // Collider 태그용으로 지정

                gridObjects[x, y] = cell;
            }
        }
    }

    /// <summary>
    /// 지정한 헥사 좌표가 유효한지 확인합니다.
    /// </summary>
    public bool IsCellExists(Vector2Int hexPos)
    {
        int x = hexPos.x;
        int y = hexPos.y;
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return false;
        return gridObjects[x, y] != null;
    }
    #endregion

    #region Public API

    /// <summary>
    /// 현재 점수에 점수를 추가합니다.
    /// </summary>
    public void AddScore(int delta = 1)
    {
        CurrentScore += delta;
      //  uiManager?.UpdateScore(CurrentScore);
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
            PlayerPrefs.SetInt("BestScore", BestScore);
        }
    }

    /// <summary>
    /// 레벨 클리어 처리 후 다음 씬 로드
    /// </summary>
    public void ClearLevel()
    {
        Level++;
        SceneManager.LoadScene(Level < 6 ? Level : 0);
    }

    /// <summary>
    /// 게임 오버 처리
    /// </summary>
    public void GameOver()
    {
        SoundManager.Instance?.GameOver();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 그리드 전체 크기를 반환합니다.
    /// </summary>
    public Vector2 GetGridDimensions()
    {
        return new Vector2(gridWidth, gridHeight);
    }
    #endregion
}
