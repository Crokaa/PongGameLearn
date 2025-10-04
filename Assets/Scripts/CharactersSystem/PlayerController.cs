using UnityEngine;

public class PlayerController : Character
{
    private float _moveVertical;

    void Update()
    {
        _moveVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(0, _moveVertical * _speed);
    }

}
