using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected AttackDetails attackDetails;

    protected D_MeleeAttack stateData;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, Transform attackPosition, D_MeleeAttack stateData) 
        : base(entity, stateMachine, animationBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damage = stateData.attackDamage;
        attackDetails.position = entity.AliveGO.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        var detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

        foreach (var item in detectedObjects)
        {
            item.transform.SendMessage("Damage", attackDetails);
        }
    }
}