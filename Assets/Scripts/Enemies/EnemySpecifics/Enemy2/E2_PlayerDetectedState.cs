using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_PlayerDetectedState : PlayerDectectedState
{
	private Enemy2 enemy;

	public E2_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy2 enemy): base(entity, stateMachine, animBoolName, stateData)
	{
		this.enemy = enemy;
	}
	public override void Enter()
	{
		base.Enter();
	}
	public override void Exit()
	{
		base.Exit();
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();

//if (performLongRangeAction)
 //       {
  //          stateMachine.ChangeState(enemy.rangedAttackState);
//}
 //       else if (!isPlayerInMaxAgroRange)
  //      {
//stateMachine.ChangeState(enemy.lookForPlayerState);
   //     }
    }
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
