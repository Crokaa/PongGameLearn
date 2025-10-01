using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Notify the GameManager that a goal has been scored
        GameManager._instance.onGoalScored();
    }
}
