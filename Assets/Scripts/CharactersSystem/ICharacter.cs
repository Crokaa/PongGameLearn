using UnityEngine;

public interface ICharacter
{
    public int Score { get; }
    public void ScoreGoal();
    public void OnCollisionEnter2D(Collision2D collision);
}
