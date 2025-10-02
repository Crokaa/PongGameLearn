using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    [SerializeField] GameObject characterThatScores;
    void OnTriggerEnter2D(Collider2D collision)
    {
        characterThatScores.GetComponent<ICharacter>().ScoreGoal();
        GameManager.instance.OnGoalScored();
    }
}
