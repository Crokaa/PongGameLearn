using UnityEngine;
public class Character : MonoBehaviour
{
    [SerializeField] protected float _speed;
    protected Rigidbody2D _rb;
    public int Score { get; private set; }
    private float _paddleHeight;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Score = 0;
        _paddleHeight = GetComponent<SpriteRenderer>().size.y * transform.localScale.y;
    }
    public void ScoreGoal()
    {
        Score++;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        float collisionPointY = collision.GetContact(0).point.y;
        float characterCenterY = transform.position.y;
        float offset = collisionPointY - characterCenterY;

        float normalizedOffset = offset / (_paddleHeight / 2); // Normalize the offset based on paddle height
        float bounceAngle = normalizedOffset * BallBehaviour.MAXANGLE; // Max bounce angle of 45 degrees

        collision.gameObject.GetComponent<BallBehaviour>().Launch((int)transform.right.x, bounceAngle);
    }
}
