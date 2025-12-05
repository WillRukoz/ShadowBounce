using UnityEngine;


public class PaddleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _velocity = 10f;

    private Rigidbody2D _rb;

    void Start()
    {
        // Obtener el Rigidbody2D si existe
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoverPaleta();
    }

    void MoverPaleta()
    {
        // Capturar input horizontal (A/D o Flechas Izquierda/Derecha)
        float movement = Input.GetAxisRaw("Horizontal");

        // Calcular nueva posición
        Vector3 currentPos = transform.position;
        currentPos.x += movement * _velocity * Time.deltaTime;

        // Aplicar la nueva posición
        if (_rb != null)
        {
            // Si tiene Rigidbody2D, usar MovePosition para mejor física
            _rb.MovePosition(currentPos);
        }
        else
        {
            // Si no, mover directamente el transform
            transform.position = currentPos;
        }
    }
}
