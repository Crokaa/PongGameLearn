using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject _ball;
    [SerializeField] GameObject _menuUI;
    [SerializeField] GameObject _scoreBoardUI;
    [SerializeField] GameObject _gameLayoutObject;
    private PlayerController _player;
    private EnemyBehaviour _enemy;
    private TextMeshProUGUI _playerScoreText;
    private TextMeshProUGUI _enemyScoreText;
    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {   
        Button[] buttons = _menuUI.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => StartCoroutine(GameStart((EnemyDifficulty)System.Enum.Parse(typeof(EnemyDifficulty), button.GetComponentInChildren<TextMeshProUGUI>().text))));
        }
    }

    IEnumerator GameStart(EnemyDifficulty difficulty)
    {

        // Canvas activation/deactivation
        _menuUI.SetActive(false);
        _scoreBoardUI.SetActive(true);
        _gameLayoutObject.SetActive(true);

        // Set up references to player and enemy
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
        _enemy.SetDifficulty(difficulty);

        // This could be achieved with SerializeField, but this way I got only the canvas as SerializeField and activate anything else from it
        TextMeshProUGUI[] scoreBoardText = _scoreBoardUI.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI scoreBoardObj in scoreBoardText)
        {
            if (scoreBoardObj.name == "PlayerScore")
                _playerScoreText = scoreBoardObj;
            else if (scoreBoardObj.name == "EnemyScore")
                _enemyScoreText = scoreBoardObj;
        }

        //Set up the score UI
        _playerScoreText.text = _player.Score.ToString();
        _enemyScoreText.text = _enemy.Score.ToString();

        // Launch the ball
        yield return new WaitForSeconds(1f);
        _ball.GetComponent<BallBehaviour>().Launch(Random.Range(0, 2) == 0 ? -1 : 1);
    }

    public void OnGoalScored(GameObject characterThatScores)
    {
        _ball.SetActive(false);

        _playerScoreText.text = _player.Score.ToString();
        _enemyScoreText.text = _enemy.Score.ToString();

        // Relaunch the ball towards the character that suffered the goal
        StartCoroutine(_ball.GetComponent<BallBehaviour>().ResetBall((int) characterThatScores.transform.right.x));
    }
}
