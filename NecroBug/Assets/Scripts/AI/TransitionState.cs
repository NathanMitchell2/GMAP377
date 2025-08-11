using UnityEngine;

public class TransitionState : IState
{
    private float transitionDuration;
    private float timer;
    private IState nextState;

    public TransitionState(float delay, IState targetState)
    {
        transitionDuration = delay;
        nextState = targetState;
    }

    public void Enter(EnemyAI enemy)
    {
        timer = 0f;
        enemy.agent.SetDestination(enemy.transform.position); 
    }

    public void Update(EnemyAI enemy)
    {
        timer += Time.deltaTime;
        if (timer >= transitionDuration)
        {
            Debug.Log(nextState);
            enemy.ChangeState(nextState);
        }
    }

    public void Exit(EnemyAI enemy) { }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance) { }
}
