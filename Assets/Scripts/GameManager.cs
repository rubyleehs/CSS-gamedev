using System.Collections;
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
    public int difficultyLevel = 2;

    // Private variables
    // private List<Enemy> enemyList;
    private bool paused = false;
    private Text infoText;
    private new Transform camera;
    private Player player;
    private GameObject menuBackdrop;

    // Code to ensure only one GameManager exists
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
        infoText.gameObject.SetActive(paused); // paused is always false here
        menuBackdrop.SetActive(false); // Disables black screen
        infoText.text = "PAUSED";

        currentState = GameState.Play;

        // Sets up the camera
        camera.position = new Vector3(5, 4, -10f);

        // Sets up the player
        player.ResetPlayer();

        // Resets difficulty
        difficultyLevel = 2;

        // Sets up the level
        levelGen.InitLevel(difficultyLevel);
        
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
        infoText.gameObject.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    // Update is called once per frame
    void Update() {
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
        if (player.hp <= 0) {
            InitGameOver();
        }

        // Generates new chunks ahead of the camera
        if (levelGen.chunkCount * levelGen.CHUNK_ROWS < camera.position.y + levelGen.CHUNK_ROWS) {
            levelGen.SpawnChunk(difficultyLevel);
        }

        // Destroys chunks that go behind the camera
        // TODO: THIS IS AN MVP METHOD. NOT ELEGANT AT ALL.
        if (camera.position.y - 18 > levelGen.chunksDestroyed * levelGen.CHUNK_ROWS) {
            levelGen.DestroyEarliestChunk();
        }

        // Updates difficulty level
        difficultyLevel = (int)Mathf.Max(2, Mathf.Ceil(levelGen.chunkCount / 8));

    }

    private void GameOver() {
        if (Input.GetKeyDown("return"))
            InitGame();
    }
}
