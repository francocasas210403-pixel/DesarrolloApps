using UnityEngine;

public class MovimientoAstronauta : MonoBehaviour
{
    public CharacterController controller;
    
    [Header("Ajustes de Movimiento")]
    public float velocidadMaxima = 8f;
    public float aceleracion = 50f;
    public float friccion = 10f;

    [Header("Ajustes de Giro")]
    public float velocidadGiro = 10f;

    [Header("Físicas (Caída)")]
    public float gravedad = -20f; // Valor para una caída rápida y realista
    
    private Vector3 velocidadActual; // Velocidad horizontal (X, Z)
    private float velocidadVertical; // Velocidad de caída (Y)
    private Vector2 inputMovimiento;

    void Update()
    {
        // 1. Obtener entrada del teclado
        inputMovimiento.x = Input.GetAxisRaw("Horizontal");
        inputMovimiento.y = Input.GetAxisRaw("Vertical");
        
        Vector3 direccionInput = new Vector3(inputMovimiento.x, 0, inputMovimiento.y).normalized;

        // 2. Aplicar Inercia (Movimiento Horizontal)
        if (direccionInput.sqrMagnitude > 0.1f) 
        {
            velocidadActual = Vector3.MoveTowards(velocidadActual, direccionInput * velocidadMaxima, aceleracion * Time.deltaTime);
        }
        else
        {
            velocidadActual = Vector3.MoveTowards(velocidadActual, Vector3.zero, friccion * Time.deltaTime);
        }

        // 3. Lógica de Gravedad (Movimiento Vertical)
        if (controller.isGrounded)
        {
            // Si está en el suelo, reseteamos la velocidad de caída
            // Usamos un valor pequeño negativo (-2) para que el sensor isGrounded funcione siempre bien
            velocidadVertical = -2f;
        }
        else
        {
            // Si está en el aire (sobre una grieta), sumamos gravedad con el tiempo
            velocidadVertical += gravedad * Time.deltaTime;
        }

        // 4. Combinar y Mover
        // Sumamos la velocidad horizontal acumulada y la vertical de la caída
        Vector3 movimientoFinal = velocidadActual;
        movimientoFinal.y = velocidadVertical;

        controller.Move(movimientoFinal * Time.deltaTime);

        // 5. Rotación Fluida (Mirar hacia donde camina)
        if (direccionInput.sqrMagnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
        }
    }
}