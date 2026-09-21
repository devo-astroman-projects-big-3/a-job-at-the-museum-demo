using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private CharacterController _controller;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        bool isRunning = movement.sqrMagnitude > 0.01f;
        _animator.SetBool("isRunning", isRunning);

        // Flip only when there is horizontal movement.
        if (horizontal < 0f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (horizontal > 0f)
        {
            _spriteRenderer.flipX = false;
        }

        movement = movement.normalized;

        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += _gravity * Time.deltaTime;
        movement.y = _verticalVelocity;

        _controller.Move(movement * _speed * Time.deltaTime);
    }
}