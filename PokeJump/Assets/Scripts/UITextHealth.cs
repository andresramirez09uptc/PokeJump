using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextHealth : MonoBehaviour
{
    public Text textComponent; // Referencia al componente de texto

    private JohnMovement johnMovement;
    
    void Start()
    {
        // Buscar el GameObject con el tag "Player" y obtener una referencia al script JohnMovement
        GameObject johnGameObject = GameObject.FindWithTag("Player");
        // Start is called before the first frame update   
        if (johnGameObject != null)
        {
            johnMovement = johnGameObject.GetComponent<JohnMovement>();
        }
        else
        {
            Debug.LogError("No se encontr� ning�n GameObject con el tag 'Player'");
            enabled = false; // Deshabilitar el script para evitar errores
            return;
        }

        // Verificar si se asign� un componente de texto en el Inspector
        if (textComponent == null)
        {
            Debug.LogError("No se asign� ning�n componente de texto en el Inspector");
            enabled = false; // Deshabilitar el script para evitar errores
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (johnMovement != null)
        {
            // Actualizar el texto con la informaci�n de salud y vidas
            textComponent.text = " Health: " + johnMovement.Health;
        }
    }
}

