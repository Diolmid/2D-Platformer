using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float knockbackSpeedX;
    [SerializeField] private float knockbackSpeedY;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float knockbackDeathSpeedX;
    [SerializeField] private float knockbackDeathSpeedY;
    [SerializeField] private float deathTorgue;

    [SerializeField] private bool applyKnockback;

    [SerializeField] private GameObject hitParticle;

    private int _playerFacingDirection;

    private float _currentHealth;
    private float _knockbackStart;

    private bool _playerOnLeft;
    private bool _knockback;

    private PlayerController _playerController;

    private GameObject _aliveGO;
    private GameObject _brokenTopGO;
    private GameObject _brokenBottomGO;

    private Rigidbody2D _rbAlive;
    private Rigidbody2D _rbBrokenTop;
    private Rigidbody2D _rbBrokenBottom;

    private Animator _aliveAnimator;

    private void Awake()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _aliveGO = transform.Find("Alive").gameObject;
        _brokenTopGO = transform.Find("Broken Top").gameObject;
        _brokenBottomGO = transform.Find("Broken Bottom").gameObject;

        _aliveAnimator = _aliveGO.GetComponent<Animator>();

        _rbAlive = _aliveGO.GetComponent<Rigidbody2D>();
        _rbBrokenTop = _brokenTopGO.GetComponent<Rigidbody2D>();
        _rbBrokenBottom = _brokenBottomGO.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;

        _aliveGO.SetActive(true);
        _brokenTopGO.SetActive(false);
        _brokenBottomGO.SetActive(false);
    }

    private void Update() 
    {
        CheckKnockback();
    }

    private void Damage(float[] attackDetails)
    {
        _currentHealth -= attackDetails[0];

        _playerFacingDirection = attackDetails[1] < _aliveGO.transform.position.x ? 1 : -1;

        Instantiate(hitParticle, _aliveGO.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

        _playerOnLeft = _playerFacingDirection == 1;

        _aliveAnimator.SetBool("playerOnLeft", _playerOnLeft);
        _aliveAnimator.SetTrigger("damage");

        if(applyKnockback && _currentHealth > 0)
            Knockback();

        if (_currentHealth < 0)
            Die();
    }

    private void Knockback()
    {
        _knockback = true;
        _knockbackStart = Time.time;
        _rbAlive.velocity = new Vector2(knockbackSpeedX * _playerFacingDirection, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if(Time.time >= _knockbackStart + knockbackDuration && _knockback)
        {
            _knockback = false;
            _rbAlive.velocity = new Vector2(0, _rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        _aliveGO.SetActive(false);
        _brokenTopGO.SetActive(true);
        _brokenBottomGO.SetActive(true);

        _brokenTopGO.transform.position = _aliveGO.transform.position;
        _brokenBottomGO.transform.position = _aliveGO.transform.position;

        _rbBrokenBottom.velocity = new Vector2(knockbackSpeedX * _playerFacingDirection, knockbackSpeedY);
        _rbBrokenTop.velocity = new Vector2(knockbackDeathSpeedX * _playerFacingDirection, knockbackDeathSpeedY);
        _rbBrokenTop.AddTorque(deathTorgue * -_playerFacingDirection, ForceMode2D.Impulse);
    }
}