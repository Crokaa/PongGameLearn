using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, ICharacter
{

    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private Vector2 ballPosition;
    public int Score { get; private set; }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ballPosition = GameObject.FindWithTag("Ball").transform.position;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(0, (ballPosition.y - transform.position.y) * speed);
    }

    public void ScoreGoal()
    {
        Score++;
    }
}
