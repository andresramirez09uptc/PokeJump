using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    // Vida total del personaje
    public int Live;
    // Salud actual del personaje
    public int Health;

    // Prefab de la bala que se disparar�
    public GameObject BulletPrefab;
    // Velocidad de movimiento del personaje
    public float Speed;
    // Fuerza de salto del personaje
    public float JumpForce;

    // Referencia al componente Rigidbody2D
    private Rigidbody2D Rigidbody2D;
    // Referencia al componente Animator
    private Animator Animator;
    // Variable para almacenar la entrada horizontal
    private float Horizontal;
    // Variable para verificar si el personaje est� en el suelo
    private bool Grounded;
    // Tiempo del �ltimo disparo realizado
    private float LastShoot;

    // M�todo Start se llama antes de la primera actualizaci�n del frame
    void Start()
    {
        // Obtener el componente Rigidbody2D adjunto al objeto
        Rigidbody2D = GetComponent<Rigidbody2D>();
        // Obtener el componente Animator adjunto al objeto
        Animator = GetComponent<Animator>();

        // Iniciar la corrutina para incrementar la salud cada segundo
        StartCoroutine(IncreaseHealthOverTime());
    }

    // M�todo Update se llama una vez por frame
    void Update()
    {
        // Obtener la entrada horizontal del jugador
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Girar el personaje dependiendo de la direcci�n del movimiento
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Establecer la animaci�n de correr
        Animator.SetBool("running", Horizontal != 0.0f);

        // Dibujar un rayo hacia abajo desde la posici�n del personaje
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        // Verificar si el rayo colisiona con el suelo
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        // Verificar si se presiona la tecla W y el personaje est� en el suelo para saltar
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Verificar si se presiona la tecla Espacio y ha pasado suficiente tiempo desde el �ltimo disparo para disparar una bala
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 1.0f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    // M�todo para realizar un salto
    private void Jump()
    {
        // Aplicar una fuerza hacia arriba al Rigidbody2D del personaje
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    // M�todo para disparar una bala
    private void Shoot()
    {
        // Direcci�n inicial de la bala
        Vector3 direction;

        // Determinar la direcci�n de la bala basada en la escala local (direcci�n en la que est� mirando el personaje)
        if (transform.localScale.x == 1.0f)
            direction = Vector3.right; // Derecha
        else
            direction = Vector3.left; // Izquierda

        // Crear una instancia de la bala y establecer su posici�n y rotaci�n
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        // Configurar la direcci�n de la bala
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    // M�todo FixedUpdate se llama en intervalos fijos y es ideal para la f�sica
    private void FixedUpdate()
    {
        // Establecer la velocidad del Rigidbody2D del personaje
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    // M�todo llamado cuando el personaje recibe un golpe
    public void Hit(int damage)
    {
        // Reducir la salud del personaje
        Health -= damage;
        Debug.Log("SALUD ACTUAL: " + Health);

        // Verificar si la salud del personaje ha llegado a 0
        if (Health <= 0)
        {
            // Reducir la cantidad de vidas del personaje
            Live--;
            if (Live == 0)
            {
                // Destruir el objeto si se han agotado todas las vidas
                Destroy(gameObject);
            }
            else
            {
                // Resetear el estado del personaje
                ResetJohn();
            }
        }
    }

    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        // Reducir la salud del personaje
        Health -= damage;
        // Verificar si la salud del personaje ha llegado a 0
        if (Health <= 0)
        {
            // Resetear el estado del personaje
            ResetJohn();
        }
    }

    // M�todo para resetear el estado del personaje
    private void ResetJohn()
    {
        // Enviar al personaje a la posici�n x = -1.2 y = 0.5
        transform.position = new Vector3(-1.2f, 0.5f, transform.position.z);

        // Resetear la salud del personaje a 100
        Health = 100;
    }

    // Corrutina para incrementar la salud del personaje cada segundo
    private IEnumerator IncreaseHealthOverTime()
    {
        while (true)
        {
            // Esperar un segundo
            yield return new WaitForSeconds(1f);

            // Verificar si la salud del personaje es menor a 100
            if (Health < 100)
            {
                if (Health == 99)
                {
                    // Incrementar la salud en 1 si la salud es 99 para no superar 100
                    Health += 1;
                }
                else
                {
                    // Incrementar la salud en 2
                    Health += 2;
                }

                // Asegurarse de que la salud no sobrepase 100
                if (Health > 100)
                {
                    Health = 100;
                }
                Debug.Log("SALUD INCREMENTADA: " + Health);
            }
        }
    }
}
