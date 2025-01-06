using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float _curveX;
    public float curveX { get => _curveX; set => _curveX = value; }
    [SerializeField, Range(-1, 1)] private float _curveY;
    public float curveY { get => _curveY; set => _curveY = value; }
    [SerializeField] private Material[] materials;
    public static ShaderController instance;
    public float transitionDuration { get => _transitionDuration; set => _transitionDuration = value; }
    private float _transitionDuration = 2f; // Duraci�n de la transici�n
    public float holdDuration { get => _holdDuration; set => _holdDuration = value; }
    private float _holdDuration = 3f; // Duraci�n de mantenerse en el mismo valor antes de cambiar
    private float timeSinceLastChange = 0f; // Tiempo transcurrido desde el �ltimo cambio
    private bool isTransitioning = false; // Flag para saber si est� en proceso de transici�n
    private Transform playerTransform; // Referencia al transform del jugador
    //Ultimo intento

    private void Start()
    {
        instance = this;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Solo actualiza los materiales si no est� en proceso de transici�n
        if (!isTransitioning)
        {
            foreach (var m in materials)
            {
                m.SetFloat(Shader.PropertyToID("_Curve_X"), _curveX);
                m.SetFloat(Shader.PropertyToID("_Curve_Y"), _curveY);
            }
        }
    }
    // Funci�n para calcular la posici�n adelantada del jugador

    public IEnumerator ChangeCurveValues()
    {
        while (true)
        {
            // Obtener la posici�n actual del jugador
            Vector3 playerPosition = playerTransform.position;

            // Guardar los valores de curvatura actuales como punto de inicio
            float startCurveX = _curveX;
            float startCurveY = _curveY;

            // Generar nuevos valores de destino
            float targetX = Random.Range(-1f, 1f);
            float targetY = Random.Range(-1f, 1f);

            // Iniciar la transici�n
            float elapsedTime = 0f;
            while (elapsedTime < transitionDuration)
            {
                // Calcular el progreso de la transici�n
                float t = elapsedTime / transitionDuration;

                // Interpolar gradualmente hacia los nuevos valores utilizando una funci�n de suavizado
                _curveX = Mathf.Lerp(startCurveX, targetX, Mathf.SmoothStep(0f, 1f, t));
                _curveY = Mathf.Lerp(startCurveY, targetY, Mathf.SmoothStep(0f, 1f, t));

                // Incrementar el tiempo transcurrido
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Asegurar que los valores sean exactamente los de destino al final de la transici�n
            _curveX = targetX;
            _curveY = targetY;

            // Tiempo de espera antes de cambiar de nuevo
            yield return new WaitForSeconds(holdDuration);
        }
    }

    public void SetZeroCurveXY()
    {
        foreach (var m in materials)
        {
            // Establece los valores de los shaders a 0
            m.SetFloat(Shader.PropertyToID("_Curve_X"), 0f);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), 0f);
        }
    }
}
