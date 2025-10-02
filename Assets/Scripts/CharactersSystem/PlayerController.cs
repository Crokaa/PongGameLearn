using System.Linq.Expressions;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField] private float _speed;
    private float _moveVertical;
    private Rigidbody2D _rb;
    public int Score { get; private set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Score = 0;
    }

    void Update()
    {
        _moveVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(0, _moveVertical * _speed);
    }
    
    public void ScoreGoal()
    {
        Score++;
    }

}
