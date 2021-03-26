using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Completed
{
    public class GameManager : MonoBehaviour
    {
        // Enum of game states
        public enum GameState
        {
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

        /// <summary>
        /// Sets up object references and the GameManager.
        /// </summary>
        void Awake()
        {
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

        /// <summary>
        /// Sets up the game.
        /// </summary>
        private void InitGame()
        {

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
            levelGen.InitLevel(0);
        }

        /// <summary>
        /// Displays game over screen and deletes level.
        /// </summary>
        private void InitGameOver()
        {

            // Destroys the level
            levelGen.DestroyLevel();

            currentState = GameState.GameOver;
            menuBackdrop.SetActive(true);
            infoText.text = "Game Over. Play again? [ENTER]";
            infoText.gameObject.SetActive(true);
        }

        /// <summary>
        /// Pause and resume. 
        /// </summary>
        public void TogglePause()
        {
            paused = !paused;
            infoText.text = "PAUSED";
            infoText.gameObject.SetActive(paused);
            Time.timeScale = paused ? 0 : 1;
        }

        /// <summary>
        /// Manages pausing and decides on which gamestate to run.
        /// (Play, GameOver)
        /// </summary>
        void Update()
        {
            // Pause Game
            if (Input.GetKeyDown("p"))
                TogglePause();

            // Game is simply not updated when paused
            if (paused)
                return;
            else
            {
                // Updates current game state
                switch (currentState)
                {
                    case GameState.Play:
                        Play();
                        break;
                    case GameState.GameOver:
                        GameOver();
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the Player, generates chunks, destroys old chunks and increases difficulty level.
        /// Active while game is being played, obviously.
        /// </summary>
        private void Play()
        {

            // Game Over if player HP is less than 0
            if (player.isDead)
            {
                InitGameOver();
            }

            // Generates new chunks ahead of the camera
            if (levelGen.chunkCount * levelGen.CHUNK_ROWS < camera.position.y + levelGen.CHUNK_ROWS)
            {
                levelGen.SpawnChunk(difficultyLevel);
            }

            // Destroys chunks that go behind the camera
            // TODO: THIS IS AN MVP METHOD. NOT ELEGANT AT ALL.
            if (camera.position.y - (levelGen.CHUNK_ROWS * 2) > levelGen.chunksDestroyed * levelGen.CHUNK_ROWS)
            {
                levelGen.DestroyEarliestChunk();
            }

            // Updates difficulty level
            difficultyLevel = Mathf.Max(difficultyLevel, levelGen.chunkCount * difficultyScaling);

        }

        /// <summary>
        /// Wait for the Enter key to be pressed, then restarts the game.
        /// Active during the GameOver screen.
        /// </summary>
        private void GameOver()
        {
            if (Input.GetKeyDown("return"))
                InitGame();
        }
    }
}
