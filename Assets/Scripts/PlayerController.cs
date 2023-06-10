using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float movementForceInAir;
    [SerializeField] private float airDragMultiplier = 0.95f;

    [Space]
    [SerializeField] private int jumpsAmount = 1;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;

    [Space]
    [SerializeField] private float wallHopForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private Vector2 wallHopDirection;
    [SerializeField] private Vector2 wallJumpDirection;

    [Space]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    [Space]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform wallCheck;

    private int _jumpsAmountLeft;
    private int _facingDirection = 1;

    private float _movementInputDirection;

    private bool _isFacingRight = true;
    private bool _isWalking;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isWallSliding;
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
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
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

        if (Input.GetButtonUp("Jump"))
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * variableJumpHeightMultiplier);
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0)
            Flip();
        else if (!_isFacingRight && _movementInputDirection > 0)
            Flip();

        _isWalking = _rb.velocity.x != 0;
    }

    private void CheckIfWallSliding()
    {
        _isWallSliding = (_isTouchingWall && !_isGrounded && _rb.velocity.y < 0);
    }

    private void CheckIfCanJump()
    {
        if ((_isGrounded && _rb.velocity.y <= 0) || _isWallSliding)
            _jumpsAmountLeft = jumpsAmount;

        _canJump = _jumpsAmountLeft > 0;
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.velocity.y);
        _animator.SetBool("isWallSliding", _isWallSliding);
    }

    private void Jump()
    {
        if (_canJump && !_isWallSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _jumpsAmountLeft--;
        }
        else if (_isWallSliding && _movementInputDirection == 0 && _canJump)
        {
            _isWallSliding = false;
            _jumpsAmountLeft--;

            var forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -_facingDirection, wallHopForce * wallHopDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if ((_isWallSliding || _isTouchingWall) && _movementInputDirection != 0 && _canJump)
        {
            _isWallSliding = false;
            _jumpsAmountLeft--;

            var forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * _movementInputDirection, wallJumpForce * wallJumpDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        if (!_isWallSliding)
        {
            _facingDirection *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void ApplyMovement()
    {
        if (_isGrounded)
            _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection != 0)
        {
            var forceToAdd = new Vector2(movementForceInAir * _movementInputDirection, 0);
            _rb.AddForce(forceToAdd);

            if (Mathf.Abs(_rb.velocity.x) > movementSpeed)
                _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);
        }
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0)
            _rb.velocity = new Vector2(_rb.velocity.x * airDragMultiplier, _rb.velocity.y);

        if (_isWallSliding)
        {
            if(_rb.velocity.y < -wallSlidingSpeed)
                _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z)); ;
    }
}