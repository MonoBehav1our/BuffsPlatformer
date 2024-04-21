using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IService
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;

    [Space][Header("Control Setting")]
    [SerializeField] private Joystick _joystick;

    [Space][Header("Move Settings")]
    [SerializeField] private float _speed;
    private float _directionX;
    private bool _flipped = false;

    [Space][Header("Jump Settings")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _groundLayers;
    private bool _isGround;

    [Space][Header("Dash Settings")]
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashDistance;
    private bool _dashing;

    [Space][Header("UpDash Settings")]
    [SerializeField] private float _upDashDistance;
    private bool _upDashingBefore;

    [Space][Header("Die Settings")]
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private GameObject _dieParticle;
    [SerializeField] private float _delayAfterDie;
    public bool Died { get { return _died; } }
    private bool _died;

    private const float MOBILESLOW = 0.75f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_dashing )
        {
            HorizontalMove();
            FaceDirection();
            VerticalDirection();
        }     
        CheckGround();
        SetAnimParametrs();
    }

    public bool Jump()
    {
        if (!_dashing)
        {
            if (_isGround)
            {
                if (Physics2D.gravity.y <= 0) _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                else _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_jumpForce);

                return true;
            }
            return false;
        }
        else return false;
    }

    public bool UpDash()
    {
        if (!_dashing && !_upDashingBefore)
        {
            if (Physics2D.gravity.y <= 0) transform.position = new Vector2(transform.position.x, transform.position.y + _upDashDistance);
            else transform.position = new Vector2(transform.position.x, transform.position.y - _upDashDistance);

            _isGround = false;
            _rigidbody.velocity = Vector2.zero;
            _upDashingBefore = true;

            return true;
        }
        else return false;
    }

    public bool Dash()
    {
        if (!_dashing)
        {
            StartCoroutine(DashCore());
            return true;
        }
        else return false;
    }

    public void Die() => StartCoroutine(DieCore());

    private void HorizontalMove()
    {
        if (!DeviceInfo.IsMobile()) _directionX = Input.GetAxis("Horizontal");
        else
        {
            if (Mathf.Abs(_joystick.Horizontal) < 0.5)
                _directionX = _joystick.Horizontal * 2 * MOBILESLOW;
            else 
                _directionX = Mathf.Sign(_joystick.Horizontal) * MOBILESLOW;
        }

        _rigidbody.velocity = new Vector2(_directionX * _speed, _rigidbody.velocity.y);
    }

    private void CheckGround()
    {
        _isGround = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayers);
        if (_isGround) _upDashingBefore = false;
    }

    private void FaceDirection()
    {
        if (_directionX < 0 && !_flipped)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _flipped = !_flipped;
        }
        else if (_directionX > 0 && _flipped)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _flipped = !_flipped;
        }
    }

    private void VerticalDirection()
    {
        if (Physics2D.gravity.y <= 0) transform.localScale = new Vector2(transform.localScale.x, Mathf.Abs(transform.localScale.y));
        else transform.localScale = new Vector2(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
    }

    private void SetAnimParametrs()
    {
        _animator.SetFloat("directionX", Mathf.Abs(_directionX));
        _animator.SetBool("isGround", _isGround);
    }

    private IEnumerator DashCore()
    {
        _dashing = true;
        float time = 0;
        Vector2 startPos = transform.position;
        Vector2 endPos;

        if (!_flipped) endPos = new Vector2(startPos.x + _dashDistance, startPos.y);
        else endPos = new Vector2(startPos.x - _dashDistance, startPos.y);

        while (time <= _dashTime) 
        {
            Vector2 newPos = Vector2.Lerp(startPos, endPos, time / _dashTime);
            _rigidbody.MovePosition(newPos);
            time += Time.deltaTime;
            yield return null;
        }

        _rigidbody.MovePosition(endPos);
        _rigidbody.velocity = Vector2.zero;
        _dashing = false;
    }

    private IEnumerator DieCore()
    {
        _died = true;
        _audioSource.clip = _dieSound;
        _audioSource.Play();

        Instantiate(_dieParticle, transform.position, transform.rotation);
        transform.position = new Vector2(1000, 1000);
        yield return new WaitForSeconds(_delayAfterDie);
        SceneLoader.Instance.DieRestart();
    }
}
