using UnityEngine;

public class PlayerController : Character
{
    private float _moveVertical;

    void Update()
    {
        if (canMove)
            _moveVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if (canMove)
            _rb.linearVelocity = new Vector2(0, _moveVertical * _speed);
    }

}
