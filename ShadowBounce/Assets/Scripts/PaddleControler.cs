using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _useTouch = true; // true para móvil, false para mouse
    
    [Header("Movement Limits")]
    [SerializeField] private float _leftLimit = -8f;
    [SerializeField] private float _rightLimit = 8f;
    
    private Camera _mainCam;
    private float _paddleWidth;
    
    void Start()
    {
        _mainCam = Camera.main;
        
        // Calcular el ancho de la paleta para ajustar límites
        if (TryGetComponent(out SpriteRenderer sr))
        {
            _paddleWidth = sr.bounds.size.x / 2f;
        }
        else if (TryGetComponent(out BoxCollider2D col))
        {
            _paddleWidth = col.bounds.size.x / 2f;
        }
    }
    
    void Update()
    {
        MovePaddle();
    }
    
    void MovePaddle()
    {
        Vector3 objPos = transform.position;
        Vector3 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        objPos.x = mousePos.x;
        
        //if (_useTouch && Input.touchCount > 0)
        //{
        //    // Control táctil para móvil
        //    Touch touch = Input.GetTouch(0);
        //    Vector3 worldPos = _mainCam.ScreenToWorldPoint(touch.position);
        //    objPos.x = worldPos.x;
        //}
        //else if (!_useTouch || Application.isEditor)
        //{
        //    //Control con mouse(útil para testing en el editor)
        //    if (Input.GetMouseButton(0))
        //    {
        //    }
        //}
        
        // Limitar movimiento dentro de los bordes
        objPos.x = Mathf.Clamp(objPos.x, _leftLimit + _paddleWidth, _rightLimit - _paddleWidth);
        
        // Interpolar suavemente hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, objPos, _speed * Time.deltaTime);
    }
    
    // Método opcional: detectar colisión con la pelota
    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Ball"))
        {
            // Aquí puedes agregar efectos de sonido o partículas
            Debug.Log("¡Pelota golpeada!");
        }
    }
}