using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        // Obtén la referencia al PlayerController
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Verifica si el playerController no es nulo y si su transform no es nulo
        if (playerController != null && playerController.transform != null)
        {
            // Posición actual del Player
            Vector3 playerPosition = playerController.transform.position;

            // Verifica si la posición en Z del Player supera el límite
            if (playerPosition.z > 2394)
            {
                // Crea una nueva posición con z en 0
                Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y, 0);

                // Establece la posición del jugador
                playerController.transform.position = newPosition;
            }
        }
    }
}
