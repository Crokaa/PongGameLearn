using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _characterThatScores;
    void OnTriggerEnter2D(Collider2D collision)
    {
        _characterThatScores.GetComponent<ICharacter>().ScoreGoal();
        GameManager.instance.OnGoalScored(_characterThatScores);
    }
}
