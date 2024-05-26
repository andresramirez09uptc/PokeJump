using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Clip de sonido que se reproducir� cuando se dispare la bala
    public AudioClip Sound;
    // Velocidad de la bala
    public float Speed;

    // Referencia al componente Rigidbody2D de la bala
    private Rigidbody2D Rigidbody2D;
    // Direcci�n en la que se mover� la bala
    private Vector2 Direction;
    // Referencia al script EnvironmentScript
    private EnvironmentScript EnvironmentScript;

    // M�todo Start se llama antes de la primera actualizaci�n del frame
    void Start()
    {
        // Encuentra el EnvironmentScript usando la etiqueta "Environment"
        EnvironmentScript = GameObject.FindWithTag("Environment").GetComponent<EnvironmentScript>();

        // Obtener el componente Rigidbody2D adjunto a la bala
        Rigidbody2D = GetComponent<Rigidbody2D>();
        // Reproducir el sonido de disparo de la bala
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    // M�todo FixedUpdate se llama en intervalos fijos y es ideal para la f�sica
    private void FixedUpdate()
    {
        // Establecer la velocidad del Rigidbody2D de la bala en la direcci�n especificada
        Rigidbody2D.velocity = Direction * Speed;
    }

    // M�todo para establecer la direcci�n de la bala
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    // M�todo para destruir la bala
    public void DestroyBullet()
    {
        // Destruir el objeto de la bala
        Destroy(gameObject);
    }

    // M�todo para enviar una actualizaci�n del puntaje al EnvironmentScript
    public void SendNewScore(int newScore)
    {
        // Aseg�rate de que EnvironmentScript no es nulo
        if (EnvironmentScript != null)
        {
            // Actualizar el puntaje en EnvironmentScript
            EnvironmentScript.UpdateScore(newScore);
        }
        else
        {
            // Imprimir un error si EnvironmentScript no se encontr�
            Debug.LogError("EnvironmentScript no encontrado");
        }
    }

    // M�todo llamado cuando la bala colisiona con otro objeto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Variable para almacenar el nuevo puntaje
        int newScore = 0;
        // Intentar obtener el script GruntScript del objeto colisionado
        GruntScript grunt = collision.GetComponent<GruntScript>();
        // Intentar obtener el script Grunt2Script del objeto colisionado
        Grunt2Script grunt2 = collision.GetComponent<Grunt2Script>();
        // Intentar obtener el script Grunt3Script del objeto colisionado
        Grunt3Script grunt3 = collision.GetComponent<Grunt3Script>();

        // Verificar si el objeto colisionado es un Grunt
        if (grunt != null)
        {
            // Llamar al m�todo Hit del Grunt
            grunt.Hit();
            // Obtener el puntaje del Grunt
            newScore = grunt.GetScore();
        }

        // Verificar si el objeto colisionado es un Grunt2
        if (grunt2 != null)
        {
            // Llamar al m�todo Hit del Grunt2
            grunt2.Hit();
            // Obtener el puntaje del Grunt2
            newScore = grunt2.GetScore();
        }

        // Verificar si el objeto colisionado es un Grunt3
        if (grunt3 != null)
        {
            // Llamar al m�todo Hit del Grunt3
            grunt3.Hit();
            // Obtener el puntaje del Grunt3
            newScore = grunt3.GetScore();
        }

        // Enviar el nuevo puntaje al EnvironmentScript
        SendNewScore(newScore);
        // Destruir la bala
        DestroyBullet();
    }
}