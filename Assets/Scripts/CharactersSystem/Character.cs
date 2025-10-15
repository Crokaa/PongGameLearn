using System;
using UnityEngine;
public class Character : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] protected AudioClip _hitBallSound;
    protected bool canMove;
    protected Rigidbody2D _rb;
    public int Score { get; private set; }
    private float _paddleHeight;
    private float _paddleWidth;
    private float _initialYPosition;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Score = 0;
        _paddleHeight = GetComponent<SpriteRenderer>().size.y * transform.localScale.y;
        _paddleWidth = GetComponent<SpriteRenderer>().size.x * transform.localScale.x;
        _initialYPosition = transform.position.y;
        canMove = true;
    }

    public void ScoreGoal()
    {
        Score++;
    }

    public void Reset()
    {
        canMove = true;
        Score = 0;
        transform.position = new Vector3(transform.position.x, _initialYPosition, transform.position.z);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;


        float ballCenterXAbs = Math.Abs(collision.transform.position.x);
        float paddleCenterXAbs = Math.Abs(transform.position.x);
        if (ballCenterXAbs > paddleCenterXAbs - (_paddleWidth / 2)) return;

        SoundFXManager.instance.PlaySoundFXClip(_hitBallSound, transform, 0.5f);

        float collisionPointY = collision.GetContact(0).point.y;
        float characterCenterY = transform.position.y;
        float offset = collisionPointY - characterCenterY;

        float normalizedOffset = offset / (_paddleHeight / 2); // Normalize the offset based on paddle height
        float bounceAngle = normalizedOffset * BallBehaviour.MAXANGLE; // Max bounce angle of 45 degrees

        collision.gameObject.GetComponent<BallBehaviour>().Launch((int)transform.right.x, bounceAngle);
    }

    public void StopMovement()
    {
        canMove = false;
        _rb.linearVelocityY = 0;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }

}
