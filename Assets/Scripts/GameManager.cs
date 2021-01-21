using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Enum of game states
    public enum GameState {
        Play,
        GameOver
    }

    // Public variables
    public GameState currentState = GameState.Play;
    public LevelGenerator levelGen;

    // Private variables
    // private List<Enemy> enemyList;
    private bool paused = false;
    private Text pauseText;
    
    // Code to ensure only one GameManager exists
    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        levelGen = GetComponent<LevelGenerator>();

        InitGame();
        Time.timeScale = 1;
    }

    private void InitGame() {
        // Inits and disables pause text
        pauseText = GameObject.Find("PauseText").GetComponent<Text>();
        pauseText.gameObject.SetActive(paused); // paused is always false here

        // TODO: Sets up the level
    }

    // Pause and resume. Input will be handled by the Player instead.
    public void TogglePause() {
        paused = !paused;
        pauseText.gameObject.SetActive(paused);
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
        // TODO
    }

    private void GameOver() {
        // TODO
    }

}
