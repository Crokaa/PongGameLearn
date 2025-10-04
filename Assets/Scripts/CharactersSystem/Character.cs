using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] protected float _speed;
    protected Rigidbody2D _rb;
    public int Score { get; private set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Score = 0;
    }
    public void ScoreGoal()
    {
        Score++;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        //TODO: check where the ball hit the character to throw it in a different direction
        float collisionPointY = collision.GetContact(0).point.y;
        if (collisionPointY > transform.position.y)
            Debug.Log("Hit Top");
        else if (collisionPointY < transform.position.y)
            Debug.Log("Hit Bottom");
        else
            Debug.Log("Hit Middle");
    }
}
