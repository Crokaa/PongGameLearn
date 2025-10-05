using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject _ball;
    [SerializeField] TextMeshProUGUI _playerScoreText;
    [SerializeField] TextMeshProUGUI _enemyScoreText;
    public static GameManager instance;
    private PlayerController _player;
    private EnemyBehaviour _enemy;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _ball.GetComponent<BallBehaviour>().Launch(Random.Range(0, 2) == 0 ? -1 : 1);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
        _playerScoreText.text = _player.Score.ToString();
        _enemyScoreText.text = _enemy.Score.ToString();
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
