using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private AudioSource _as;
    [SerializeField] private AudioClip _playerSound, _brickSound, _deadZoneSound;
    private Vector2 _velocity;
    private Vector2 _startPos;


    private void Start()
    {
        _startPos = transform.position;
        ResetBall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            _as.clip = _deadZoneSound;
            _as.Play();
            GameManager.instance.LoseHealth();
        }

        if (collision.gameObject.GetComponent<Player>() || collision.gameObject.CompareTag("Wall"))
        {
            _as.clip = _playerSound;
            _as.Play();
        }

        if (collision.gameObject.GetComponent<Brick>())
        {
            _as.clip = _brickSound;
            _as.Play();
        }
    }

    public void ResetBall()
    {
        transform.position = _startPos;
        _rb.velocity = Vector2.zero;
        _velocity.x = Random.Range(-1f, 1f);
        _velocity.y = 1;
        _rb.AddForce(_velocity * _speed);
    }
}
