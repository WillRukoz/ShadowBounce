using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Configuración del Bloque")]
    [SerializeField] private int puntosAlDestruir = 10;
    [SerializeField] private int golpesNecesarios = 1;
    [SerializeField] private Color[] coloresPorGolpe; // Colores según golpes restantes
    
    [Header("Efectos Visuales")]
    [SerializeField] private GameObject efectoDestruccion; // Prefab de partículas (opcional)
    [SerializeField] private bool escalarAlGolpear = true;
    [SerializeField] private float duracionEscala = 0.1f;
    
    [Header("Audio (Opcional)")]
    [SerializeField] private AudioClip sonidoGolpe;
    [SerializeField] private AudioClip sonidoDestruccion;
    
    private int golpesRestantes;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;
    private AudioSource audioSource;
    
    void Start()
    {
        golpesRestantes = golpesNecesarios;
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaOriginal = transform.localScale;
        
        // Configurar audio si existe
        audioSource = GetComponent<AudioSource>();
        
        // Establecer color inicial
        ActualizarApariencia();
    }
    
    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Ball"))
        {
            RecibirGolpe();
        }
    }
    
    void RecibirGolpe()
    {
        golpesRestantes--;
        
        if (golpesRestantes <= 0)
        {
            DestruirBloque();
        }
        else
        {
            // Bloque todavía tiene golpes restantes
            ActualizarApariencia();
            //ReproducirSonido(sonidoGolpe);
            
            if (escalarAlGolpear)
            {
                StartCoroutine(EfectoEscala());
            }
        }
    }
    
    void DestruirBloque()
    {
        // Agregar puntos
        // GameManager.Instance.AgregarPuntos(puntosAlDestruir);
        Debug.Log($"¡Bloque destruido! +{puntosAlDestruir} puntos");
        
        // Reproducir sonido de destrucción
        //ReproducirSonido(sonidoDestruccion);
        
        // Crear efecto de partículas
        if (efectoDestruccion != null)
        {
            Instantiate(efectoDestruccion, transform.position, Quaternion.identity);
        }
        
        // Notificar al GameManager
        // GameManager.Instance.BloqueDestruido();
        
        // Destruir el bloque
        Destroy(gameObject);
    }
    
    void ActualizarApariencia()
    {
        if (spriteRenderer == null || coloresPorGolpe == null || coloresPorGolpe.Length == 0)
            return;
        
        // Cambiar color según golpes restantes
        int indiceColor = Mathf.Max(0, golpesNecesarios - golpesRestantes);
        if (indiceColor < coloresPorGolpe.Length)
        {
            spriteRenderer.color = coloresPorGolpe[indiceColor];
        }
    }
    
    System.Collections.IEnumerator EfectoEscala()
    {
        float tiempoTranscurrido = 0f;
        Vector3 escalaGolpe = escalaOriginal * 0.9f;
        
        // Comprimir
        while (tiempoTranscurrido < duracionEscala / 2)
        {
            transform.localScale = Vector3.Lerp(escalaOriginal, escalaGolpe, tiempoTranscurrido / (duracionEscala / 2));
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        
        tiempoTranscurrido = 0f;
        
        // Expandir
        while (tiempoTranscurrido < duracionEscala / 2)
        {
            transform.localScale = Vector3.Lerp(escalaGolpe, escalaOriginal, tiempoTranscurrido / (duracionEscala / 2));
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = escalaOriginal;
    }
    
    //void ReproducirSonido(AudioClip clip)
    //{
    //    if (audioSource != null && clip != null)
    //    {
    //        audioSource.PlayOneShot(clip);
    //    }
    //}
    
    // Método para configurar el bloque desde código
    public void ConfigurarBloque(int puntos, int golpes, Color[] colores = null)
    {
        puntosAlDestruir = puntos;
        golpesNecesarios = golpes;
        golpesRestantes = golpes;
        
        if (colores != null && colores.Length > 0)
        {
            coloresPorGolpe = colores;
        }
        
        ActualizarApariencia();
    }
}