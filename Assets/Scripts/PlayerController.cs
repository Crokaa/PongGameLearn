using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float _moveVertical;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _moveVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(0, _moveVertical * speed);
    }

}
