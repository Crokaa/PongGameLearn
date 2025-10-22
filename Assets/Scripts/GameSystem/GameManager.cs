using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _scoreBoardUI;
    [SerializeField] private GameObject _gameLayoutObject;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private Texture _winImage;
    [SerializeField] private Texture _loseImage;
    [SerializeField] private PlayerController _player;
    [SerializeField] private EnemyBehaviour _enemy;
    [SerializeField] private AudioClip _inGameMusic;
    [SerializeField] private GameObject _itemSpawner;
    public GameState CurrentGameState { get; private set; }
    private TextMeshProUGUI _playerScoreText;
    private TextMeshProUGUI _enemyScoreText;
    private EnemyDifficulty _currentDifficulty;
    public static GameManager instance;
    private static readonly int GOALSTOWIN = 7;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        SetOnClickMenuButtons();
        SetOnClickGameOverButtons();
        SetOnClickPauseButtons();
        SetScoreText();
    }

    void Start()
    {
        ShowMenu();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentGameState == GameState.InGame)
                Pause();
            else if (CurrentGameState == GameState.Pause)
                Unpause();
        }
    }

    void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator GameStart(EnemyDifficulty difficulty)
    {
        HideCursor();
        _player.Reset();
        _enemy.Reset();

        // Activate _item spawner and set difficulty
        _itemSpawner.SetActive(true);
        _itemSpawner.GetComponent<ItemSpawner>().ResetItems();
        _itemSpawner.GetComponent<ItemSpawner>().EnemyDifficultyChange(difficulty);

        // Even if it's the same difficulty I will update it (this isn't a heavy operation)
        _enemy.SetDifficulty(difficulty);
        _currentDifficulty = difficulty;

        // Set up the score UI
        _playerScoreText.text = _player.Score.ToString();
        _enemyScoreText.text = _enemy.Score.ToString();

        ShowGame();

        yield return new WaitForSeconds(1f);

        // Launch the ball
        _ball.GetComponent<BallBehaviour>().Launch(Random.Range(0, 2) == 0 ? -1 : 1);
    }

    public void OnGoalScored(GameObject characterThatScores)
    {
        _ball.SetActive(false);

        _playerScoreText.text = _player.Score.ToString();
        _enemyScoreText.text = _enemy.Score.ToString();

        if (_player.Score == GOALSTOWIN || _enemy.Score == GOALSTOWIN)
        {
            GameOver();
        }
        else
        {
            // Relaunch the ball towards the character that suffered the goal
            StartCoroutine(_ball.GetComponent<BallBehaviour>().ResetBall((int)characterThatScores.transform.right.x));
        }
    }

    private void GameOver()
    {
        //Deactivate item spawner
        _itemSpawner.SetActive(true);

        _enemy.StopMovement();
        _player.StopMovement();

        // They are always an image, but assigning null removes the error.
        // If by any chance there's a break it means there's not 2 Images in my canvas
        RawImage playerImage = null;
        RawImage enemyImage = null;

        // Same as before. Could be done with SerializeField
        RawImage[] images = _gameOverUI.GetComponentsInChildren<RawImage>();
        foreach (RawImage image in images)
        {
            if (image.name == "PlayerImage")
                playerImage = image;
            else if (image.name == "EnemyImage")
                enemyImage = image;
        }

        if (_player.Score == GOALSTOWIN)
        {
            playerImage.texture = _winImage;
            enemyImage.texture = _loseImage;
        }
        else
        {
            playerImage.texture = _loseImage;
            enemyImage.texture = _winImage;
        }

        ShowCursor();
        ShowGameOver();
    }

    private void ShowMenu()
    {
        _scoreBoardUI.SetActive(false);
        _gameLayoutObject.SetActive(false);
        _gameOverUI.SetActive(false);
        _menuUI.SetActive(true);

        CurrentGameState = GameState.Menu;
    }

    private void ShowGame()
    {
        _menuUI.SetActive(false);
        _gameOverUI.SetActive(false);
        _scoreBoardUI.SetActive(true);
        _gameLayoutObject.SetActive(true);
        _pauseUI.SetActive(false);

        CurrentGameState = GameState.InGame;

        // Since this is also part of UI it makes sense to be in this fuction
        _ball.GetComponent<BallBehaviour>().ResetBallPosition();
        _ball.SetActive(true);
    }

    private void ShowGameOver()
    {
        _gameOverUI.SetActive(true);
        CurrentGameState = GameState.GameOver;
    }

    private void SetOnClickMenuButtons()
    {
        Button[] menuButtons = _menuUI.GetComponentsInChildren<Button>();
        foreach (Button button in menuButtons)
        {
            if (button.name == "QuitGameButton")
                button.onClick.AddListener(() => ExitGame());
            else
                button.onClick.AddListener(() => StartCoroutine(GameStart((EnemyDifficulty)System.Enum.Parse(typeof(EnemyDifficulty), button.GetComponentInChildren<TextMeshProUGUI>().text))));
        }
    }

    private void SetOnClickGameOverButtons()
    {
        Button[] gameOverButtons = _gameOverUI.GetComponentsInChildren<Button>();
        foreach (Button button in gameOverButtons)
        {
            if (button.name == "RetryButton")
                button.onClick.AddListener(() => StartCoroutine(GameStart(_currentDifficulty)));
            else if (button.name == "MainMenuButton")
                button.onClick.AddListener(ShowMenu);
            else if (button.name == "QuitGameButton")
                button.onClick.AddListener(() => ExitGame());
        }
    }

    private void SetOnClickPauseButtons()
    {

        Button[] pauseButtons = _pauseUI.GetComponentsInChildren<Button>();
        foreach (Button button in pauseButtons)
        {
            if (button.name == "ResumeButton")
                button.onClick.AddListener(() => Unpause());
            else if (button.name == "QuitGameButton")
                button.onClick.AddListener(() => ExitGame());
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        _player.GetComponent<Character>().StopMovement();
        _enemy.GetComponent<Character>().StopMovement();

        ShowCursor();
        _pauseUI.SetActive(true);
        CurrentGameState = GameState.Pause;
    }

    private void Unpause()
    {
        HideCursor();
        _pauseUI.SetActive(false);
        CurrentGameState = GameState.InGame;

        Time.timeScale = 1f;
        _player.GetComponent<Character>().ResumeMovement();
        _enemy.GetComponent<Character>().ResumeMovement();
    }

    private void SetScoreText()
    {
        TextMeshProUGUI[] scoreBoardText = _scoreBoardUI.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI scoreBoardObj in scoreBoardText)
        {
            if (scoreBoardObj.name == "PlayerScore")
                _playerScoreText = scoreBoardObj;
            else if (scoreBoardObj.name == "EnemyScore")
                _enemyScoreText = scoreBoardObj;
        }
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

public enum GameState
    {
        Menu, InGame, GameOver, Pause
    }
