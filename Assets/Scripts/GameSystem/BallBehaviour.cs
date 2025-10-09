using System.Collections;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private static WaitForSeconds _waitOneSecond = new WaitForSeconds(1f);
    private Rigidbody2D _rb;
    [SerializeField] private float _initialSpeed;
    private float _currentSpeed;
    public static float MAXANGLE = 45f; // Max bounce angle in degrees

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.position = Vector2.zero;
        _currentSpeed = _initialSpeed;
    }

    void FixedUpdate()
    {
        // Keep the ball speed constant
        if (gameObject.activeSelf)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _currentSpeed;
    }

    // This is cleaner than passing a bool, the ball doesn't need to know the concept of player or enemy
    public void Launch(int directionX, float angle = float.NaN)
    {
        float randomAngle = Random.Range(-MAXANGLE, MAXANGLE);
        float angleInRad = float.IsNaN(angle) ? randomAngle * Mathf.Deg2Rad : angle * Mathf.Deg2Rad;

        float vecX = Mathf.Cos(angleInRad);
        float vecY = Mathf.Sin(angleInRad);

        Vector2 direction;

        // directionX is either 1 or -1 and vecX is always positive 
        direction = new Vector2(directionX * vecX, vecY).normalized;

        _rb.linearVelocity = direction * _currentSpeed;
    }

    public void ResetBallPosition()
    {
        _rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;

        _currentSpeed = _initialSpeed;
    }

    public IEnumerator ResetBall(int directionX)
    {
        ResetBallPosition();

        yield return _waitOneSecond;

        //Object will be invisible after a goal so I have to make it visible again
        gameObject.SetActive(true);

        // Wait for a second before relaunching the ball
        yield return _waitOneSecond;

        Launch(directionX);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            if (_currentSpeed < 15f)
                _currentSpeed += 0.5f;
        }
    }

}
