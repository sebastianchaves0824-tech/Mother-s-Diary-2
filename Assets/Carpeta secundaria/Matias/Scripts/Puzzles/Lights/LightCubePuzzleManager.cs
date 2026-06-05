using System.Collections.Generic;
using UnityEngine;


// Obligamos a que el Manager tenga un AudioSource
[RequireComponent(typeof(AudioSource))]
public class LightCubePuzzleManager : MonoBehaviour
{
    [Header("Secuencia del Puzzle")]
    [Tooltip("Arrastra aquí los cubos en el orden exacto en el que deben encenderse.")]
    public List<LightCube> correctSequence;

    [Header("Recompensa")]
    [Tooltip("El prefab de la llave (debe tener tu script KeyItem).")]
    public GameObject keyPrefab;
    [Tooltip("El lugar exacto donde aparecerá la llave al resolver el puzzle.")]
    public Transform keySpawnPoint;

    [Header("Audio (Feedback)")]
    [Tooltip("Sonido cuando el jugador enciende la roca correcta.")]
    public AudioClip correctSound;
    [Tooltip("Sonido cuando el jugador se equivoca de roca.")]
    public AudioClip errorSound;
    [Tooltip("Sonido cuando el jugador resuelve todo el puzzle.")]
    public AudioClip solvedSound;

    private AudioSource audioSource;
    private int currentStep = 0;
    [HideInInspector] public bool isSolved = false;

    private void Start()
    {
        // Obtenemos el AudioSource del Manager
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public bool OnCubeInteracted(LightCube clickedCube)
    {
        if (clickedCube == correctSequence[currentStep])
        {
            // --- ACIERTO ---
            // Reproducimos el sonido de éxito
            if (correctSound != null)
                audioSource.PlayOneShot(correctSound);

            clickedCube.TurnOn();
            currentStep++;

            if (currentStep >= correctSequence.Count)
            {
                SolvePuzzle();
            }

            return true;
        }
        else
        {
            // --- ERROR ---
            // Reproducimos el sonido de error
            if (errorSound != null)
                audioSource.PlayOneShot(errorSound);

            ResetPuzzle();

            return false;
        }
    }

    private void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("¡Puzzle resuelto correctamente!");

        if (solvedSound != null)
        {
            audioSource.PlayOneShot(solvedSound);
        }

        if (keyPrefab != null && keySpawnPoint != null)
        {
            Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Falta asignar el prefab de la llave o el punto de aparición en el Manager.");
        }
    }

    private void ResetPuzzle()
    {
        Debug.Log("Orden incorrecto. Apagando cubos y reiniciando secuencia.");
        currentStep = 0;

        foreach (LightCube cube in correctSequence)
        {
            cube.TurnOff();
        }
    }
}