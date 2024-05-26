using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class EnvironmentScript : MonoBehaviour
{
    // Puntaje del jugador
    public int Score;

    // Prefabs de los enemigos que se generar�n
    public GameObject GruntPrefab;
    public GameObject Grunt2Prefab;
    public GameObject Grunt3Prefab;

    // �ltimo prefab generado
    private GameObject lastSpawnedPrefab;

    // Generador de n�meros aleatorios
    System.Random random = new System.Random();
    // �ndice para recorrer los datos del CSV
    private int IndexCsv;
    // Lista para almacenar los datos del CSV
    private List<float> csvData;

    // M�todo Start se llama antes de la primera actualizaci�n del frame
    void Start()
    {
        // Combina la ruta de la aplicaci�n con el nombre del archivo CSV
        string filePath = Path.Combine(Application.dataPath, "Scripts/numeros_ri_final.csv");
        // Lee los datos del CSV y los almacena en la lista
        csvData = ReadCsv(filePath);
        // Inicializa IndexCsv con un valor aleatorio dentro del rango permitido
        IndexCsv = random.Next(0, Mathf.Min(50000, csvData.Count));
        // Inicializar el primer enemigo como Grunt
        lastSpawnedPrefab = GruntPrefab;
        // Inicia la invocaci�n repetida para generar un enemigo cada 5 segundos
        InvokeRepeating("SpawnGrunt", 0f, 5f);
    }

    // M�todo Update se llama una vez por frame
    void Update()
    {
        // No se realiza ninguna acci�n en Update actualmente
    }

    // M�todo para leer el CSV y almacenar los datos en una lista
    private List<float> ReadCsv(string filePath)
    {
        // Lista para almacenar los datos le�dos del CSV
        List<float> data = new List<float>();

        // Verifica si el archivo existe
        if (File.Exists(filePath))
        {
            // Crea un StreamReader para leer el archivo
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = "";
                // Itera sobre cada l�nea del archivo
                while (!reader.EndOfStream)
                {
                    // Lee la l�nea actual
                    line = reader.ReadLine();
                    // Parsear la l�nea a float utilizando CultureInfo.InvariantCulture
                    float lineFloat = float.Parse(line, CultureInfo.InvariantCulture);
                    // A�adir el valor a la lista de datos
                    data.Add(lineFloat);
                }
            }
        }
        else
        {
            // Muestra un mensaje de error si el archivo no existe
            Debug.LogError("El archivo CSV no existe en la ruta especificada: " + filePath);
        }

        // Retorna la lista de datos
        return data;
    }

    // M�todo para obtener una cantidad determinada de datos desde IndexCsv y actualizar IndexCsv
    public List<float> GetCsvData(int count)
    {
        // Lista para almacenar los datos resultantes
        List<float> result = new List<float>();

        // Verifica que IndexCsv est� dentro del rango v�lido
        if (IndexCsv >= 0 && IndexCsv < csvData.Count)
        {
            // Calcula el �ndice final asegur�ndose de no exceder el tama�o de csvData
            int endIndex = Mathf.Min(IndexCsv + count, csvData.Count);

            // A�ade los datos desde IndexCsv hasta endIndex a la lista result
            for (int i = IndexCsv; i < endIndex; i++)
            {
                result.Add(csvData[i]);
            }

            // Actualiza IndexCsv a la �ltima posici�n obtenida
            IndexCsv = endIndex;
        }
        else
        {
            // Muestra una advertencia si IndexCsv est� fuera del rango v�lido
            Debug.LogWarning("IndexCsv est� fuera del rango v�lido.");
        }

        // Retorna la lista de datos resultantes
        return result;
    }

    // M�todo para obtener el siguiente dato del CSV
    public float GetNextCsvData()
    {
        // Verifica si hay m�s datos en la lista csvData
        if (IndexCsv < csvData.Count)
        {
            // Obtiene el siguiente valor
            float value = csvData[IndexCsv];
            // Incrementa IndexCsv
            IndexCsv++;
            // Retorna el valor obtenido
            return value;
        }
        else
        {
            // Muestra una advertencia si no hay m�s datos
            Debug.LogWarning("No hay m�s datos en el CSV.");
            // Retorna un valor por defecto
            return 0f;
        }
    }

    // M�todo para actualizar el puntaje
    public void UpdateScore(int newScore)
    {
        // Incrementa el puntaje
        Score += newScore;
        // Muestra el puntaje actual en la consola
        Debug.Log("PUNTAJE ACTUAL: " + Score);
    }

    // M�todo para generar un enemigo basado en el �ltimo generado
    private void SpawnGrunt()
    {
        // Posici�n donde se generar� el enemigo
        Vector3 spawnPosition = new Vector3(1f, -0.3f, 0f);
        // Prefab por defecto a generar
        GameObject prefabToSpawn = GruntPrefab;

        // Generar n�mero pseudoaleatorio utilizando los datos del CSV
        float randomValue = GetNextCsvData();

        // Determinar qu� enemigo generar basado en el �ltimo generado y el n�mero aleatorio
        if (lastSpawnedPrefab == GruntPrefab)
        {
            if (randomValue >= 0f && randomValue < 0.38f)
            {
                prefabToSpawn = GruntPrefab;
            }
            else if (randomValue >= 0.38f && randomValue < 0.63f)
            {
                prefabToSpawn = Grunt2Prefab;
            }
            else if (randomValue >= 0.63f && randomValue <= 1f)
            {
                prefabToSpawn = Grunt3Prefab;
            }
        }
        else if (lastSpawnedPrefab == Grunt2Prefab)
        {
            if (randomValue >= 0f && randomValue < 0.22f)
            {
                prefabToSpawn = GruntPrefab;
            }
            else if (randomValue >= 0.22f && randomValue < 0.62f)
            {
                prefabToSpawn = Grunt2Prefab;
            }
            else if (randomValue >= 0.62f && randomValue <= 1f)
            {
                prefabToSpawn = Grunt3Prefab;
            }
        }
        else if (lastSpawnedPrefab == Grunt3Prefab)
        {
            if (randomValue >= 0f && randomValue < 0.45f)
            {
                prefabToSpawn = GruntPrefab;
            }
            else if (randomValue >= 0.45f && randomValue < 0.74f)
            {
                prefabToSpawn = Grunt2Prefab;
            }
            else if (randomValue >= 0.74f && randomValue <= 1f)
            {
                prefabToSpawn = Grunt3Prefab;
            }
        }

        // Instanciar el prefab seleccionado en la posici�n especificada
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Actualizar el �ltimo prefab generado
        lastSpawnedPrefab = prefabToSpawn;
    }
}
