using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject ball;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI enemyScoreText;
    public static GameManager instance;
    private PlayerController _player;
    private EnemyController _enemy;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        ball.GetComponent<BallBehaviour>().Launch(enemy: Random.Range(0, 2) == 0);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        playerScoreText.text = _player.Score.ToString();
        enemyScoreText.text = _enemy.Score.ToString();
    }

    public void OnGoalScored()
    {
        ball.SetActive(false);
        
        playerScoreText.text = _player.Score.ToString();
        enemyScoreText.text = _enemy.Score.ToString();

        StartCoroutine(ball.GetComponent<BallBehaviour>().ResetBall());
    }
    
}
