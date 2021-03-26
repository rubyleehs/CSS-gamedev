﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    // Enum of game states
    public enum GameState {
        Play,
        GameOver
    }

    // Public variables
    public GameState currentState;
    public LevelGenerator levelGen;
    public float startingDifficultyLevel = 2;    
    public float difficultyScaling = 0.2f;

    [HideInInspector]
    public float difficultyLevel = 2;

    // Private variables
    // private List<Enemy> enemyList;
    private bool paused = false;
    private Text infoText;
    private new Transform camera;
    private Player player;
    private GameObject menuBackdrop;

    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        levelGen = GetComponent<LevelGenerator>();

        // Inits references
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
        menuBackdrop = GameObject.Find("MenuBackdrop");
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Player>();

        InitGame();
        Time.timeScale = 1;
    }

    private void InitGame() {

        // Sets up the game
        paused = false;
        infoText.gameObject.SetActive(paused); // paused is always false here
        menuBackdrop.SetActive(paused); // Disables black screen
        
        currentState = GameState.Play;

        // Sets up the camera
        camera.position = new Vector3(10, 5, -10f);

        // Sets up the player
        player.ResetStats();

        // Resets difficulty
        difficultyLevel = startingDifficultyLevel;

        // Sets up the level
        levelGen.InitLevel();   
    }

    private void InitGameOver() {

        // Destroys the level
        levelGen.DestroyLevel();

        currentState = GameState.GameOver;
        menuBackdrop.SetActive(true);
        infoText.text = "Game Over. Play again? [ENTER]";
        infoText.gameObject.SetActive(true);
    }

    // Pause and resume. Input will be handled by the Player instead.
    public void TogglePause() {
        paused = !paused;
        infoText.text = "PAUSED";
        infoText.gameObject.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    // Update is called once per frame
    void Update() {
        // Pause Game
        if (Input.GetKeyDown("p"))
            TogglePause();

        // Game is simply not updated when paused
        if (paused)
            return;
        else {
            // Updates current game state
            switch (currentState) {
                case GameState.Play:
                    Play();
                    break;
                case GameState.GameOver:
                    GameOver();
                    break;
            }
        }
    }

    private void Play() {

        // Game Over if player HP is less than 0
        if (player.isDead) {
            InitGameOver();
        }

        // Generates new chunks ahead of the camera
        if (levelGen.chunkCount * levelGen.CHUNK_ROWS < camera.position.y + levelGen.CHUNK_ROWS) {
            levelGen.SpawnChunk(difficultyLevel);
        }

        // Destroys chunks that go behind the camera
        // TODO: THIS IS AN MVP METHOD. NOT ELEGANT AT ALL.
        if (camera.position.y - (levelGen.CHUNK_ROWS * 2) > levelGen.chunksDestroyed * levelGen.CHUNK_ROWS) {
            levelGen.DestroyEarliestChunk();
        }

        // Updates difficulty level
        difficultyLevel = Mathf.Max(difficultyLevel, levelGen.chunkCount * difficultyScaling);

    }

    private void GameOver() {
        if (Input.GetKeyDown("return"))
            InitGame();
    }
}
