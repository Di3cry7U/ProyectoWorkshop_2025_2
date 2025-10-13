using System.Collections;
using UnityEngine;

public class Mezclas : MonoBehaviour
{
    [Header("Configuracion del matraz")]
    [Range(20f, 180f)] 
    public float anguloDeVertido = 60f; // �ngulo (en el eje Z) necesario para que el vertido se active
    public float duracionDelVertido = 1.0f; // Tiempo que la Corrutina esperar�, simulando la duraci�n del flujo
    public float fuerzaDeVertido = 2f; 
    public GameObject prefabLiquido; 
    public Transform spawnPointLiquido; 

    [Header("Configuracion de mezcla")]
    public Color colorMezclaInicial = Color.blue; 
    public Color colorMezclaFinal = Color.green; 

    Rigidbody rb; 

    bool estaVertiendo = false; // True si la Corrutina de vertido est� activa
    bool estaMezclando = false; // True si la mezcla ha ocurrido o est� ocurriendo
    Renderer matrazRenderer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody no encontrado, el matraz necesita un Rigidbody");
        }

        // Obtiene el Renderer para poder cambiar el color del material
        matrazRenderer = GetComponent<Renderer>();
        if (matrazRenderer != null)
        {
            //establecer el color inicial del material del matraz
            matrazRenderer.material.color = colorMezclaInicial;
        }
        else
        {
            Debug.LogError("Renderer no encontrado en el matraz");
        }

        // Comprobaci�n de seguridad: verifica si el punto de salida ha sido asignado
        if (spawnPointLiquido == null)
        {
            Debug.LogError("!ADVERTENCIA VR�: Spawn Point Liquido no asignado");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Obtiene la rotaci�n local del objeto en el eje Z. 
        float currentAngle = transform.localEulerAngles.z;

        // Esto permite detectar la inclinaci�n sin importar si se gira a la izquierda o derecha.
        if (currentAngle > 180f)
        {
            currentAngle = 360f - currentAngle;
        }

        // Condici�n de Activaci�n: Si el �ngulo es suficiente Y el matraz no est� ya vertiendo o mezclado.
        if (currentAngle >= anguloDeVertido && !estaVertiendo && !estaMezclando)
        {
           
            StartCoroutine(VerterLiquido_Coroutine());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Comprueba 3 condiciones para iniciar la mezcla:
        // 1. El objeto de colisi�n tiene el Tag "Matraz".
        // 2. ESTE matraz estaba activamente vertiendo l�quido.
        // 3. La mezcla a�n no ha ocurrido.
        if (collision.gameObject.CompareTag("Matraz") && estaVertiendo && !estaMezclando)
        {
            // Detiene la Corrutina de vertido para que el flujo se detenga inmediatamente al chocar/mezclarse.
            StopAllCoroutines();
            // Inicia la Corrutina de mezcla, obteniendo el script 'Mezclas' del objeto chocado.
            StartCoroutine(MezclarYCambiarColor_Coroutine(collision.gameObject.GetComponent<Mezclas>()));
        }
    }
    // Corrutina para simular el vertido del l�quido
    IEnumerator VerterLiquido_Coroutine()
    {
        estaVertiendo = true; 
        Debug.Log(gameObject.name + ": �Vertindo liquido!");

        GameObject liquidoInstance = null;
        // Si el prefab y el punto de salida est�n asignados:
        if (prefabLiquido != null && spawnPointLiquido != null)
        {
            // Crea una instancia del prefab del l�quido en la boca del matraz
            liquidoInstance = Instantiate(prefabLiquido, spawnPointLiquido.position, spawnPointLiquido.rotation);

            // Obtiene el Rigidbody del l�quido reci�n instanciado
            Rigidbody rbLiquido = liquidoInstance.GetComponent<Rigidbody>();
            if (rbLiquido != null)
            {
                // Aplica una fuerza de impulso para que el chorro salga proyectado
                rbLiquido.AddForce(spawnPointLiquido.forward * fuerzaDeVertido, ForceMode.Impulse);
            }
        }
        // Pausa la ejecuci�n aqu� y espera la duraci�n definida (simulando el flujo)
        yield return new WaitForSeconds(duracionDelVertido);
        // L�gica de limpieza: si la mezcla NO ocurri�, destruye el l�quido instanciado
        if (!estaMezclando && liquidoInstance != null)
        {
            Destroy(liquidoInstance);
        }

        estaVertiendo = false; // Baja la bandera de vertido, permitiendo un nuevo vertido
    }
    // Corrutina que gestiona el cambio de color para simular la mezcla
    IEnumerator MezclarYCambiarColor_Coroutine(Mezclas otroMatraz)
    {
        estaMezclando = true; 
        Debug.Log("!MEZCLA DETECTAD� Cambiando color");

        // 1. Cambio de color para ESTE matraz
        if (matrazRenderer != null)
        {
            float t = 0f;
            Color colorInicio = matrazRenderer.material.color; // Captura el color actual

            while (t < 1) // Bucle de animaci�n: se ejecuta hasta que el progreso 't' llega a 1 (100%)
            {
                t += Time.deltaTime * 3; // Incrementa el progreso (3 controla la velocidad de la animaci�n)
                // Interpola el color suavemente entre el color inicial y el color final (efecto de mezcla gradual)
                matrazRenderer.material.color = Color.Lerp(colorInicio, colorMezclaFinal, t);
                yield return null; // Pausa y espera el siguiente frame (necesario para la animaci�n en Corrutinas)
            }
        }

        // 2. Cambio de color para el OTRO matraz
        // Solo se ejecuta si la referencia al otro script no es nula
        if (otroMatraz != null && otroMatraz.matrazRenderer != null)
        {
            // Calcula el nuevo color de mezcla promediando los colores finales de ambos matraces
            Color nuevaMezcla = (colorMezclaFinal + otroMatraz.colorMezclaFinal) / 2;

            float t = 0;
            Color colorInicioOtro = otroMatraz.matrazRenderer.material.color;

            while (t < 1)
            {
                t += Time.deltaTime * 3;
                otroMatraz.matrazRenderer.material.color = Color.Lerp(colorInicioOtro, nuevaMezcla, t);
                yield return null;
            }

            otroMatraz.estaMezclando = true;
        }
    }
}
