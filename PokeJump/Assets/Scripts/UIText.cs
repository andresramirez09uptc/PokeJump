using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public Text textComponent; // Referencia al componente de texto

    private JohnMovement johnMovement;

    void Start()
    {
        // Buscar el GameObject con el tag "Player" y obtener una referencia al script JohnMovement
        GameObject johnGameObject = GameObject.FindWithTag("Player");
        if (johnGameObject != null)
        {
            johnMovement = johnGameObject.GetComponent<JohnMovement>();
        }
        else
        {
            Debug.LogError("No se encontró ningún GameObject con el tag 'Player'");
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
        if (johnMovement != null)
        {
            // Actualizar el texto con la información de salud y vidas
            textComponent.text = " Lives: " + johnMovement.Live;
        }
    }
}
