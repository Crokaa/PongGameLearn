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
    [SerializeField] private Texture _winImage;
    [SerializeField] private Texture _loseImage;
    [SerializeField] private PlayerController _player;
    [SerializeField] private EnemyBehaviour _enemy;
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

        Button[] menuButtons = _menuUI.GetComponentsInChildren<Button>();
        foreach (Button button in menuButtons)
        {
            button.onClick.AddListener(() => StartCoroutine(GameStart((EnemyDifficulty)System.Enum.Parse(typeof(EnemyDifficulty), button.GetComponentInChildren<TextMeshProUGUI>().text))));
        }

        TextMeshProUGUI[] scoreBoardText = _scoreBoardUI.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI scoreBoardObj in scoreBoardText)
        {
            if (scoreBoardObj.name == "PlayerScore")
                _playerScoreText = scoreBoardObj;
            else if (scoreBoardObj.name == "EnemyScore")
                _enemyScoreText = scoreBoardObj;
        }

        Button[] gameOverButtons = _gameOverUI.GetComponentsInChildren<Button>();
        foreach (Button button in gameOverButtons)
        {
            if (button.name == "RetryButton")
                button.onClick.AddListener(() => StartCoroutine(GameStart(_currentDifficulty)));
            else if (button.name == "MainMenuButton")
                button.onClick.AddListener(ShowMenu);
        }
    }

    void Start()
    {
        ShowMenu();
    }

    IEnumerator GameStart(EnemyDifficulty difficulty)
    {
        _player.Reset();
        _enemy.Reset();

        // Even if it's the same difficulty I will update it (this isn't a heavy operation)
        _enemy.SetDifficulty(difficulty);
        _currentDifficulty = difficulty;

        // Set up the score UI
        _playerScoreText.text = _player.Score.ToString();
        _enemyScoreText.text = _enemy.Score.ToString();

        ShowGame();

        yield return new WaitForSeconds(1f);

        // Launch the ball
        _ball.GetComponent<BallBehaviour>().Launch(Random.Range(0, 2) == 0? -1 : 1);
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

        ShowGameOver();
    }

    private void ShowMenu()
    {
        _scoreBoardUI.SetActive(false);
        _gameLayoutObject.SetActive(false);
        _gameOverUI.SetActive(false);
        _menuUI.SetActive(true);
    }

    private void ShowGame()
    {
        _menuUI.SetActive(false);
        _gameOverUI.SetActive(false);
        _scoreBoardUI.SetActive(true);
        _gameLayoutObject.SetActive(true);

        // Since this is also part of UI it makes sense to be in this fuction
        _ball.GetComponent<BallBehaviour>().ResetBallPosition();
        _ball.SetActive(true);
    }

    private void ShowGameOver()
    {
        _gameOverUI.SetActive(true);
    }

}
