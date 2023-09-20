using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private float inputTimer;
    [SerializeField] private float attack1Radius;
    [SerializeField] private float attack1Damage;
    [SerializeField] private float stunDamageAmount = 1;

    [SerializeField] private Transform attack1HitBoxPosition;
    [SerializeField] private LayerMask whatIsDamageable;
 
    [SerializeField] private bool combatEnabled;

    private float _lastInputTime = Mathf.Infinity;
    private AttackDetails _attackDetails;

    private bool _gotInput;
    private bool _isAttacking;
    private bool _isFirstAttack;

    private PlayerController _playerController;
    private PlayerStats _playerStats;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("canAttack", combatEnabled);
        _playerController = GetComponent<PlayerController>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttack();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                _gotInput = true;
                _lastInputTime = Time.time;
            }

        }
    }

    private void CheckAttack()
    {
        if (_gotInput)
        {
            if (!_isAttacking)
            {
                _gotInput = false;
                _isAttacking = true;
                _isFirstAttack = !_isFirstAttack;
                _animator.SetBool("attack1", true);
                _animator.SetBool("firstAttack", _isFirstAttack);
                _animator.SetBool("isAttacking", _isAttacking);
            }
        }

        if(Time.time > _lastInputTime + inputTimer)
            _gotInput = false;
    }

    private void CheckAttackHitBox()
    {
        var detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPosition.position, attack1Radius, whatIsDamageable);

        _attackDetails.damage = attack1Damage;
        _attackDetails.position = transform.position;
        _attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (var detectedObject in detectedObjects)
        {
            detectedObject.transform.parent.SendMessage("Damage", _attackDetails);
        }
    }

    private void FinishAttack1()
    {
        _isAttacking = false;
        _animator.SetBool("isAttacking", _isAttacking);
        _animator.SetBool("attack1", false);
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (_playerController.GetDashStatus())
            return;

        int direction = attackDetails.position.x < transform.position.x ? 1 : -1;

        _playerStats.DecreaseHealth(attackDetails.damage);

        _playerController.Knockback(direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPosition.position, attack1Radius);
    }
}