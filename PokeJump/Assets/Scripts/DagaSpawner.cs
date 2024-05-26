using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DagaSpawner : MonoBehaviour
{
    // Prefab de la daga que se generará
    public GameObject DagaPrefab;

    //public float speed; // Velocidad de caída de la daga
    public int damage; // Daño que causa al jugador

    public float spawnInterval; // Intervalo base para la generación de dagas
    private float timer; // Temporizador para el control del intervalo de generación

    private Queue<float> positionsX; // Cola para almacenar las posiciones X de las dagas desde el CSV
    private Queue<float> arrivalTimes; // Cola para almacenar los tiempos de llegada desde el CSV
    private float simulationTime = 0f; // Tiempo de simulación actual

    // Método Start se llama antes de la primera actualización del frame
    void Start()
    {
        // Cargar las posiciones X desde el archivo CSV y almacenarlas en una cola
        positionsX = new Queue<float>(LoadPositionsFromCSV("Assets/positions.csv"));
        // Cargar los tiempos de llegada desde el archivo CSV y almacenarlos en una cola
        arrivalTimes = new Queue<float>(LoadArrivalTimesFromCSV("Assets/arrivalTimes.csv"));

        // Verificar que el prefab de Daga está asignado
        if (DagaPrefab == null)
        {
            Debug.LogError("DagaPrefab no está asignado en el inspector.");
        }
    }

    // Método Update se llama una vez por frame
    void Update()
    {
        // Incrementar el tiempo de simulación y el temporizador
        simulationTime += Time.deltaTime;
        timer += Time.deltaTime;

        // Generar dagas basado en los tiempos de llegada de la teoría de colas
        if (arrivalTimes.Count > 0 && simulationTime >= arrivalTimes.Peek())
        {
            float arrivalTime = arrivalTimes.Dequeue(); // Obtener el tiempo de llegada procesado
            SpawnDaga(arrivalTime); // Generar la daga
        }

        // Reseteo del temporizador para el siguiente intervalo de generación
        if (timer >= spawnInterval)
        {
            timer = 0f;
        }
    }

    // Método para generar una daga en la posición y tiempo especificado
    void SpawnDaga(float arrivalTime)
    {
        if (positionsX != null && positionsX.Count > 0)
        {
            float positionX = positionsX.Dequeue(); // Usar y remover la posición en la cola
            Vector3 spawnPosition = new Vector3(positionX, 0.6f, 0.0f); // Posición de generación de la daga
            GameObject daga = Instantiate(DagaPrefab, spawnPosition, Quaternion.identity); // Instanciar la daga

            // Configurar la velocidad y el daño de la daga
            DagaController dagaController = daga.GetComponent<DagaController>();
            if (dagaController == null)
            {
                dagaController = daga.AddComponent<DagaController>();
            }
            //dagaController.speed = speed;
            dagaController.damage = damage;

            // Reinserta la posición al final de la cola para reusarla
            positionsX.Enqueue(positionX);
        }
        else
        {
            Debug.LogError("No hay posiciones disponibles para generar dagas.");
        }
    }

    // Método para cargar posiciones desde un archivo CSV
    List<float> LoadPositionsFromCSV(string filePath)
    {
        List<float> positions = new List<float>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    float position;
                    if (float.TryParse(line, out position))
                    {
                        positions.Add(position);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("El archivo no pudo ser leído:");
            Debug.LogError(e.Message);
        }

        return positions;
    }

    // Método para cargar tiempos de llegada desde un archivo CSV
    List<float> LoadArrivalTimesFromCSV(string filePath)
    {
        List<float> times = new List<float>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    float time;
                    if (float.TryParse(line, out time))
                    {
                        times.Add(time);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("El archivo no pudo ser leído:");
            Debug.LogError(e.Message);
        }

        return times;
    }
}

// Clase controladora de la daga
public class DagaController : MonoBehaviour
{
    //public float speed; // Velocidad de caída de la daga
    public int damage; // Daño que causa al jugador

    // Método Update se llama una vez por frame
    void Update()
    {
        // Hacer que la daga se mueva hacia abajo
        transform.Translate(Vector2.down * Time.deltaTime);

        // Destruir la daga si cae fuera de la pantalla
        if (transform.position.y < -0.4f)
        {
            Destroy(gameObject);
        }
    }

    // Método para manejar la colisión de la daga con otros objetos
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Lógica de colisión con el jugador
            collision.GetComponent<JohnMovement>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
