using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.position = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Keep the ball speed constant
        if (gameObject.activeSelf)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _speed;
    }

    // This is cleaner than passing a bool, the ball doesn't need to know the concept of player or enemy
    public void Launch(float directionX)
    {

        Vector2 direction;
        // Even if directionX is -1 range swaps the values
        float randomX = Random.Range(0f, directionX);
        float randomY = Random.Range(-1f, 1f);

        direction = new Vector2(randomX, randomY).normalized;

        _rb.linearVelocity = direction * _speed;
    }

    public IEnumerator ResetBall(float directionX)
    {
        _rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;

        yield return new WaitForSeconds(1f);

        //Object will be invisible after a goal so I have to make it visible again
        gameObject.SetActive(true);

        // Wait for a second before relaunching the ball
        yield return new WaitForSeconds(1f);

        Launch(directionX);
    }

}
