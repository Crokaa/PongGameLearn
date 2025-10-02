using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BallBehaviour: MonoBehaviour
{
    Rigidbody2D _rb;
    public float speed = 3f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.position = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Keep the ball speed constant
        _rb.linearVelocity = _rb.linearVelocity.normalized * speed;
    }

    public void Launch(bool enemy = false)
    {
        Vector2 direction;
        float directionX = Random.Range(0, 1f);
        float directionY = Random.Range(-1f, 1f);

        if (enemy)
        {
            direction = new Vector2(-directionX, directionY).normalized;
        }
        else
        {
            direction = new Vector2(directionX, directionY).normalized;
        }


        _rb.linearVelocity = direction * speed;
    }

    public IEnumerator ResetBall()
    {
        _rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;

        //yield return new WaitForSeconds(1f);
        
        //Object will be invisible after a goal so I have to make it visible again
        gameObject.SetActive(true);

        // Wait for a second before relaunching the ball
        yield return new WaitForSeconds(1f);

        // #TODO: The ball will be thrown towards the player who conceded the goal
        Launch(Random.Range(0, 2) == 0);
    }

}
