using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject ball;
    public static GameManager _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        ball.GetComponent<BallMovement>().Launch(enemy: Random.Range(0, 2) == 0);
    }

    public void onGoalScored()
    {
        //TODO: Add score
        Debug.Log("Goal Scored");
    }
    
}
