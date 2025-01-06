using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using RandomUnity = UnityEngine.Random;
public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    public static GameManager instance;
    private bool _gameStarted = false;
    [HideInInspector]public bool gameStarted { get => _gameStarted; set => _gameStarted = value; }
    private Animator animator;
    [NonSerialized] private bool _canMove = true;
    public bool canMove { get => _canMove; set => _canMove = value; }
    public bool gameEnded { get => _gameEnded; set => _gameEnded = value; }
    [NonSerialized] private bool _gameEnded = false;
    public float timeStart = 3.5f;
    private Camera mainCamera;
    private void Awake()
    {
        instance = this;
        StartCoroutine(StartGameDelay(timeStart));
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("CurveLevel", timeStart);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStart();
        SetCamera();
    }

    IEnumerator StartGameDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        StartGame(); // Después de 3 segundos, llama al método StartGame
    }

    private void CheckStart()
    {
        if (_gameStarted && canMove)
        {
            //animator.SetTrigger("RunDelay");
            PlayerController.instance.GetSwipe();
            PlayerController.instance.SetPlayerPosition();
            PlayerController.instance.MovePlayer();
            PlayerController.instance.Jump();
            PlayerController.instance.Roll();
            //MovingTrains.instance.MovTrains();
        }
        else
        {
            if (timeStart >= 0)
                timeStart -= Time.deltaTime; //Actualizar temp
        }
    }

    private void CurveLevel()
    {
        ShaderController.instance.StartCoroutine(ShaderController.instance.ChangeCurveValues());
    }

    public void StartGame()
    {
        _gameStarted = true;
    }

    void OnGUI()
    {
        if (!_gameStarted)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 50;
            style.alignment = TextAnchor.MiddleCenter; // Alinea el texto al centro

            // Calcula la posición centrada en la pantalla
            Rect labelRect = new Rect((Screen.width - 200) / 2, (Screen.height - 50) / 2, 200, 50); // 200 y 50 son el ancho y alto del temporizador respectivamente

            // Muestra el temporizador en la pantalla del juego
            GUI.Label(labelRect, timeStart.ToString("f0"), style); // Formatea el tiempo para mostrar solo un decimal
        }

        if (gameEnded)
        {
            // Estilo del botón
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.fontSize = 50;

            // Tamaño y la posición del botón
            float buttonWidth = 200f;
            float buttonHeight = 50f;
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            Rect buttonRect = new Rect(screenWidth - buttonWidth - 40f, 10f, buttonWidth, buttonHeight);

            // Botón en la interfaz
            if (GUI.Button(buttonRect, "Restart", style2))
            {
                LoadScene("Prototype");
            }
            void LoadScene(string sceneName)
            {
                SceneManager.LoadScene(sceneName); //Se cargará la escena Prototype
            }
        }
    }

    public void GameOver()
    {
        canMove = false;
        gameEnded = true;
        ShaderController.instance.curveX = 0;
        ShaderController.instance.curveY = 0;
        ShaderController.instance.StopAllCoroutines();
        ShaderController.instance.SetZeroCurveXY();
    }

    public void SetCamera()
    {
        if (gameStarted == false)
        {
            mainCamera.transform.position = new Vector3(0f, 1.26600003f, -3.82999992f);
        }
        else if (gameStarted == true) 
        {
            mainCamera.transform.position = new Vector3 (0f, 2.24000001f, -3.82999992f);
        }
    }
}

