using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Asegúrate de incluir esta línea

public class UITextScore : MonoBehaviour
{
    public Text textComponent; // Referencia al componente de texto

    private EnvironmentScript environmentScript;

    void Start()
    {
        // Buscar el GameObject con el tag "Environment" y obtener una referencia al script EnvironmentScore
        GameObject environmentScriptObject = GameObject.FindWithTag("Environment");
        if (environmentScriptObject != null)
        {
            environmentScript = environmentScriptObject.GetComponent<EnvironmentScript>();
        }
        else
        {
            Debug.LogError("No se encontró ningún GameObject con el tag 'Environment'");
            enabled = false; // Deshabilitar el script para evitar errores
            return;
        }

        // Verificar si se asignó un componente de texto en el Inspector
        if (textComponent == null)
        {
            Debug.LogError("No se asignó ningún componente de texto en el Inspector");
            enabled = false; // Deshabilitar el script para evitar errores
            return;
        }
    }

    void Update()
    {
        if (environmentScript != null)
        {
            // Actualizar el texto con la información del score
            textComponent.text = "Score: " + environmentScript.Score;
        }
    }
}