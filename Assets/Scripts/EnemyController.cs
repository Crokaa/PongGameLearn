using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private Vector2 ballPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
}
