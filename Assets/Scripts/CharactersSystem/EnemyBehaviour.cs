using UnityEngine;

public class EnemyBehaviour : Character
{
    private GameObject _ball;
    private float _ballYPos;
    void Start()
    {
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

}

