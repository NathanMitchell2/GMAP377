using UnityEngine;

public interface IState
{
    void Enter(EnemyAI enemy);
    void Update(EnemyAI enemy);
    void Exit(EnemyAI enemy);
    void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance);
}
