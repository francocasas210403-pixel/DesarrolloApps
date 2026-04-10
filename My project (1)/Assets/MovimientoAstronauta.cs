using UnityEngine;

public class MovimientoAstronauta : MonoBehaviour
{
    public CharacterController controller;
    public float velocidad = 5f;
    private float gravedad = -9.81f; // Fuerza de gravedad
    private Vector3 velocidadVertical; 

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        // Aplicar Movimiento Horizontal
        if (direccion.magnitude >= 0.1f)
        {
            controller.Move(direccion * velocidad * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direccion);
        }

        // --- SOLUCIÓN ELEVACIÓN: GRAVEDAD ---
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f; // Lo mantiene pegado al suelo
        }

        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }
}