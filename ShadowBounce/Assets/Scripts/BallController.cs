using UnityEngine;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _initialSpeed = 8f;
    [SerializeField] private float _maxSpeed = 35f;
    [SerializeField] private float _speedRate = 0.1f;
    
    [Header("Initial Settings")]
    [SerializeField] private bool _isBallOnPaddle = true;
    [SerializeField] private Transform _paddle;
    [SerializeField] private Vector3 _offsetPaddle = new Vector3(0, 0.5f, 0);
    
    [Header("Limits")]
    [SerializeField] private float _lowerLimit = -10f;
    
    private Rigidbody2D _rb;
    private bool _isBallLaunched = false;
    private Vector2 _lastDir;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        if (_isBallOnPaddle && _paddle != null)
        {
            _rb.isKinematic = true;
        }
        else
        {
            LaunchBall();
        }
    }
    
    void Update()
    {
        // Iniciar juego con toque o clic
        if (!_isBallLaunched && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
        
        // Mantener pelota pegada a la paleta antes de lanzar
        if (!_isBallLaunched && _paddle != null)
        {
            transform.position = _paddle.position + _offsetPaddle;
        }
        
        // Verificar si la pelota cayó
        if (transform.position.y < _lowerLimit)
        {
            LoseLife();
        }
        
        // Mantener velocidad constante
        if (_isBallLaunched && _rb.velocity.magnitude > 0)
        {
            _rb.velocity = _rb.velocity.normalized * Mathf.Min(_rb.velocity.magnitude, _maxSpeed);
        }
    }
    
    void LaunchBall()
    {
        if (_isBallLaunched) return;
        
        _isBallLaunched = true;
        _rb.isKinematic = false;
        
        // Lanzar en dirección aleatoria hacia arriba
        float randomAngle = Random.Range(-45f, 45f);
        Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        _rb.velocity = dir * _initialSpeed;
        
        _lastDir = _rb.velocity.normalized;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ajustar ángulo de rebote con la paleta
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Block") ||
            collision.gameObject.CompareTag("Wall"))
        {
            SetBounceAngle(collision);
        }

        //Incrementar velocidad ligeramente con cada rebote
        if (_rb.velocity.magnitude < _maxSpeed)
        {
            _rb.velocity *= (_rb.velocity.magnitude + _speedRate);
        }

        // Guardar última dirección válida
        _lastDir = _rb.velocity.normalized;
        
        // Evitar que la pelota se quede horizontal
        AdjustHorizontalAngle();
    }
    
    void SetBounceAngle(Collision2D colision)
    {
        // Calcular posición relativa del impacto
        float relativePos = (transform.position.x - colision.transform.position.x) /
                                 colision.collider.bounds.size.x;

        // Ajustar ángulo basado en dónde golpeó (-1 izquierda, 0 centro, 1 derecha)
        float hitAngle = relativePos * 60f; // Máximo 60 grados

        Vector2 dir = Quaternion.Euler(0, 0, hitAngle) * Vector2.up;
        _rb.velocity = dir * _rb.velocity.magnitude;
    }
    
    void AdjustHorizontalAngle()
    {
        // Si la pelota está muy horizontal, ajustar ángulo
        float currentAngle = Vector2.Angle(_rb.velocity, Vector2.right);
        
        if (currentAngle < 15f || currentAngle > 165f)
        {
            float ySign = Mathf.Sign(_rb.velocity.y);
            if (ySign == 0) ySign = 1;
            
            Vector2 nuevaDireccion = new Vector2(_rb.velocity.x, ySign * Mathf.Abs(_rb.velocity.x) * 0.5f);
            _rb.velocity = nuevaDireccion.normalized * _rb.velocity.magnitude;
        }
    }
    
    void LoseLife()
    {
        Debug.Log("¡Perdiste una vida!");
        
        // Reiniciar posición
        _isBallLaunched = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        
        if (_paddle != null)
        {
            transform.position = _paddle.position + _offsetPaddle;
        }
        
        // Aquí puedes agregar lógica de vidas
        // GameManager.Instance.PerderVida();
    }
    
    // Método público para reiniciar
    public void Reset()
    {
        LoseLife();
    }
}