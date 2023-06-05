using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;

    [Space]
    [SerializeField] private int jumpsAmount = 1;
    [SerializeField] private float jumpForce = 16f;

    [Space]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    private int _jumpsAmountLeft;

    private float _movementInputDirection;

    private bool _isFacingRight = true;
    private bool _isWalking;
    private bool _isGrounded;
    private bool _canJump;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _jumpsAmountLeft = jumpsAmount;
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0)
            Flip();
        else if (!_isFacingRight && _movementInputDirection > 0)
            Flip();

        _isWalking = _rb.velocity.x != 0;
    }

    private void CheckIfCanJump()
    {
        if (_isGrounded && _rb.velocity.y <= 0)
            _jumpsAmountLeft = jumpsAmount;

        _canJump = _jumpsAmountLeft > 0;
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.velocity.y);
    }

    private void Jump()
    {
        if (_canJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _jumpsAmountLeft--;
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void ApplyMovement()
    {
        _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}