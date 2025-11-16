using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadInicial = 8f;
    [SerializeField] private float velocidadMaxima = 15f;
    [SerializeField] private float incrementoVelocidad = 0.1f;
    
    [Header("Configuración Inicial")]
    [SerializeField] private bool iniciarConPaleta = true;
    [SerializeField] private Transform paleta;
    [SerializeField] private Vector3 offsetPaleta = new Vector3(0, 0.5f, 0);
    
    [Header("Límites y Rebotes")]
    [SerializeField] private float limiteInferior = -10f;
    
    private Rigidbody2D rb;
    private bool juegoIniciado = false;
    private Vector2 ultimaDireccion;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (iniciarConPaleta && paleta != null)
        {
            rb.isKinematic = true;
        }
        else
        {
            LanzarPelota();
        }
    }
    
    void Update()
    {
        // Iniciar juego con toque o clic
        if (!juegoIniciado && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            LanzarPelota();
        }
        
        // Mantener pelota pegada a la paleta antes de lanzar
        if (!juegoIniciado && paleta != null)
        {
            transform.position = paleta.position + offsetPaleta;
        }
        
        // Verificar si la pelota cayó
        if (transform.position.y < limiteInferior)
        {
            PerderVida();
        }
        
        // Mantener velocidad constante
        if (juegoIniciado && rb.velocity.magnitude > 0)
        {
            rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude, velocidadMaxima);
        }
    }
    
    void LanzarPelota()
    {
        if (juegoIniciado) return;
        
        juegoIniciado = true;
        rb.isKinematic = false;
        
        // Lanzar en dirección aleatoria hacia arriba
        float anguloAleatorio = Random.Range(-45f, 45f);
        Vector2 direccion = Quaternion.Euler(0, 0, anguloAleatorio) * Vector2.up;
        rb.velocity = direccion * velocidadInicial;
        
        ultimaDireccion = rb.velocity.normalized;
    }
    
    void OnCollisionEnter2D(Collision2D colision)
    {
        // Ajustar ángulo de rebote con la paleta
        if (colision.gameObject.CompareTag("Player"))
        {
            AjustarRebotePaleta(colision);
        }
        
        // Incrementar velocidad ligeramente con cada rebote
        if (rb.velocity.magnitude < velocidadMaxima)
        {
            rb.velocity *= (1f + incrementoVelocidad);
        }
        
        // Guardar última dirección válida
        ultimaDireccion = rb.velocity.normalized;
        
        // Evitar que la pelota se quede horizontal
        CorregirAnguloHorizontal();
    }
    
    void AjustarRebotePaleta(Collision2D colision)
    {
        // Calcular posición relativa del impacto
        float posicionRelativa = (transform.position.x - colision.transform.position.x) / 
                                 colision.collider.bounds.size.x;
        
        // Ajustar ángulo basado en dónde golpeó (-1 izquierda, 0 centro, 1 derecha)
        float anguloRebote = posicionRelativa * 60f; // Máximo 60 grados
        
        Vector2 direccion = Quaternion.Euler(0, 0, anguloRebote) * Vector2.up;
        rb.velocity = direccion * rb.velocity.magnitude;
    }
    
    void CorregirAnguloHorizontal()
    {
        // Si la pelota está muy horizontal, ajustar ángulo
        float anguloActual = Vector2.Angle(rb.velocity, Vector2.right);
        
        if (anguloActual < 15f || anguloActual > 165f)
        {
            float signoY = Mathf.Sign(rb.velocity.y);
            if (signoY == 0) signoY = 1;
            
            Vector2 nuevaDireccion = new Vector2(rb.velocity.x, signoY * Mathf.Abs(rb.velocity.x) * 0.5f);
            rb.velocity = nuevaDireccion.normalized * rb.velocity.magnitude;
        }
    }
    
    void PerderVida()
    {
        Debug.Log("¡Perdiste una vida!");
        
        // Reiniciar posición
        juegoIniciado = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        
        if (paleta != null)
        {
            transform.position = paleta.position + offsetPaleta;
        }
        
        // Aquí puedes agregar lógica de vidas
        // GameManager.Instance.PerderVida();
    }
    
    // Método público para reiniciar
    public void Reiniciar()
    {
        PerderVida();
    }
}