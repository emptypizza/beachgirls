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
    // Singleton implementation
    public static GameManager Instance;

    // UI Manager
    //[SerializeField]
    public UIManager UIManager;

    // Gameplay variables
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
        // Singleton Pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    private void InitializeGame()
    {

        Debug.LogFormat("Loading a new level {0}...", nLevel);
        GS = GameState.Ready;
        nGameScore_current = 0;
        nGameScore_Best = 0;
        fGametime = 0f;
        Time.timeScale = 1f;

        LeftLimit = -8.9f;
        RightLimit = 9;
        TopLimit = 30f;
        BottomLimit = -9f;
        if (GameManager.nLevel >= 1)
            SoundManager.Instance.PlayBackgroundMusic(nLevel);
    }

    private void Start()
    {

        // Initialize UI Manager
        UIManager = FindObjectOfType<UIManager>();
        if (UIManager == null)
            Debug.LogError("UIManager not found.");

        // Additional initializations can go here
        InitializeGame();
    }

    private void Update()
    {
        switch (GS)
        {
            case GameState.Play:
                UpdateGameplay();
                break;
            case GameState.Pause:
                PauseGame();
                break;
            case GameState.Clear:
                UIManager.GameClear();
                break;
            case GameState.Gameover:
                // Time.timeScale = 0.05f;
             //   UIManager.GameOver();
                break;
        }
    }

    private void UpdateGameplay()
    {
        // Update the game time
        fGametime += Time.deltaTime;

        // Other gameplay update logic can go here
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        // Other pause logic can go here
    }

    public void ClearGame()
    {
        nLevel += 1;

        if (nLevel < 6)
            SceneManager.LoadScene(nLevel);
        else
            SceneManager.LoadScene(0);


    }


    public void GameOver()
    {


        SoundManager.Instance.GameOver();
        SceneManager.LoadScene(0);
        //GS = GameState.Gameover;

    }


    public void AddScore(int scoreToAdd)
    {
        nGameScore_current += scoreToAdd;
    }
    public void AddScore()
    {
        nGameScore_current += 1;

    }



}

