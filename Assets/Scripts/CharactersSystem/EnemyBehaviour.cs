using UnityEngine;

public class EnemyBehaviour : Character
{
    private GameObject _ball;
    private float _ballYPos;
    private static readonly float EASYSPEED = 5f;
    private static readonly float MEDIUMSPEED = 8f;
    private static readonly float HARDSPEED = 12f;
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


    public void SetDifficulty(EnemyDifficulty difficulty)
    {
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                _speed = EASYSPEED;
                break;
            case EnemyDifficulty.Medium:
                _speed = MEDIUMSPEED;
                break;
            case EnemyDifficulty.Hard:
                _speed = HARDSPEED;
                break;
            // No need for default (all cases are covered and worst case _speed is the same as the one from SerializeField)
        }
    }
}

