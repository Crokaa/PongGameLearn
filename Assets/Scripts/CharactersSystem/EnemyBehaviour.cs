using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, ICharacter
{

    [SerializeField] private float _speed;
    private Rigidbody2D _rb;
    private GameObject _ball;
    private float _ballYPos;
    public int Score { get; private set; }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Score = 0;
        _ball = GameObject.FindWithTag("Ball");
    }

    void Update()
    {
        if (_ball == null || !_ball.activeSelf) return;
        
        _ballYPos = _ball.transform.position.y;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(0, (_ballYPos - transform.position.y) * _speed);
    }

    public void ScoreGoal()
    {
        Score++;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: check where the ball hit the character to throw it in a different direction
    }
}
