using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttackState stateData;

    protected Projectile projectile;
    protected GameObject projectileGO;

    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, Transform attackPosition, D_RangeAttackState stateData) 
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

        projectileGO = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        projectile = projectileGO.GetComponent<Projectile>();
        projectile.SetupProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);
    }
}