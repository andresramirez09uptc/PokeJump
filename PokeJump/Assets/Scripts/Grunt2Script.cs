using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grunt2Script : MonoBehaviour
{

    // Prefab de la bala que el Grunt2 disparará
    public GameObject BulletGruntPrefab;

    // Salud del Grunt2
    private int Health = 1;
    private int Damage = 10;
    private float Pause = 2.5f;
    private int Score = 10;

    // Referencia al objeto del jugador (John)
    private GameObject John;
    // Tiempo del último disparo realizado
    private float LastShoot;

    private EnvironmentScript environmentScript;

    // Método Start se llama antes de la primera actualización del frame
    void Start()
    {
        // Encuentra el objeto del jugador utilizando su etiqueta
        John = GameObject.FindWithTag("Player");

        environmentScript = FindObjectOfType<EnvironmentScript>();
    }

    // Método Update se llama una vez por frame
    void Update()
    {
        // Si John no está asignado, no hacer nada
        if (John == null) return;

        // Obtener la dirección hacia John
        Vector3 direction = John.transform.position - transform.position;

        // Verificar si están en la misma posición en el eje Y y a una distancia menor o igual a 1.0f en el eje X
        if (Mathf.Abs(direction.y) < 0.1f && Mathf.Abs(direction.x) <= 1.0f)
        {
            // Girar el Grunt hacia John
            if (direction.x >= 0.0f)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Mirando a la derecha
            else
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // Mirando a la izquierda
        }

        // Calcular la distancia en el eje X
        float distance = Mathf.Abs(direction.x);

        // Disparar si John está en la misma posición en el eje Y y a una distancia menor o igual a 0.8f en el eje X
        if (Time.time > LastShoot + Pause)
        {
            if (Mathf.Abs(direction.y) < 0.1f && distance < 0.8f)
            {
                Shoot();
                LastShoot = Time.time; // Actualizar el tiempo del último disparo
            }
        }
        MoveBasedOnCsvData();
    }

    private void MoveBasedOnCsvData()
    {
        if (environmentScript != null)
        {
            float moveValue = environmentScript.GetNextCsvData(); // Obtener el siguiente valor del CSV

            // Mover a la izquierda si el valor es menor a 0.5, a la derecha si es mayor o igual a 0.5
            if (moveValue < 0.5f)
            {
                transform.Translate(Vector3.left * Time.deltaTime * 3);
            }
            else
            {
                transform.Translate(Vector3.right * Time.deltaTime * 3);
            }
        }
    }

    // Método para disparar una bala
    private void Shoot()
    {
        // Dirección inicial de la bala
        Vector3 direction;

        // Determinar la dirección de la bala basada en la escala local (dirección en la que está mirando el Grunt)
        if (transform.localScale.x == 1.0f)
            direction = Vector3.right; // Derecha
        else
            direction = Vector3.left; // Izquierda

        // Crear una instancia de la bala y establecer su posición y rotación
        GameObject bulletGrunt = Instantiate(BulletGruntPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        // Configurar la dirección de la bala
        bulletGrunt.GetComponent<BulletGruntScript>().SetDamage(Damage);
        bulletGrunt.GetComponent<BulletGruntScript>().SetDirection(direction);
    }

    // Método llamado cuando el Grunt recibe un golpe
    public void Hit()
    {
        // Reducir la salud del Grunt
        Health--;
        // Destruir el objeto si la salud llega a 0
        if (Health == 0) Destroy(gameObject);
    }

    public int GetScore()
    {
        return Score;
    }
}