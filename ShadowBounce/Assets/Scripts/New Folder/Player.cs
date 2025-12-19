using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed;
    private float inputValue;
    private Vector2 _dir;
    private Vector2 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }
    void Update()
    {
        inputValue = Input.GetAxisRaw("Horizontal");

        if(inputValue == 1)
        {
            _dir = Vector2.right;
        }
        else if(inputValue == -1)
        {
            _dir = Vector2.left;
        }
        else
        {
            _dir = Vector2.zero;
        }

        _rb.AddForce(_dir * _moveSpeed * Time.deltaTime * 100);
    }

    public void ResetPlayer()
    {
        transform.position = _startPos;
        _rb.velocity = Vector2.zero;
    }
}
