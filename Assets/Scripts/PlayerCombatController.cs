using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private float inputTimer;
    [SerializeField] private float attack1Radius;
    [SerializeField] private float attack1Damage;

    [SerializeField] private Transform attack1HitBoxPosition;
    [SerializeField] private LayerMask whatIsDamageable;
 
    [SerializeField] private bool combatEnabled;

    private float _lastInputTime = Mathf.Infinity;

    private bool _gotInput;
    private bool _isAttacking;
    private bool _isFirstAttack;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("canAttack", combatEnabled);
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

        foreach (var detectedObject in detectedObjects)
        {
            detectedObject.transform.parent.SendMessage("Damage", attack1Damage);

        }
    }

    private void FinishAttack1()
    {
        _isAttacking = false;
        _animator.SetBool("isAttacking", _isAttacking);
        _animator.SetBool("attack1", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPosition.position, attack1Radius);
    }
}