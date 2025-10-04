using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _initialSpeed;
    private float _currentSpeed;

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
    public void Launch(float directionX, float directionY = 0)
    {

        Vector2 direction;
        float randomY = directionY;
        // Even if directionX is -1 range swaps the values
        float randomX = Random.Range(0f, directionX);
        if (directionY == 0)
            randomY = Random.Range(-1f, 1f);

        direction = new Vector2(randomX, randomY).normalized;

        _rb.linearVelocity = direction * _currentSpeed;
    }

    public IEnumerator ResetBall(float directionX)
    {
        _rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;

        _currentSpeed = _initialSpeed;

        yield return new WaitForSeconds(1f);

        //Object will be invisible after a goal so I have to make it visible again
        gameObject.SetActive(true);

        // Wait for a second before relaunching the ball
        yield return new WaitForSeconds(1f);

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
