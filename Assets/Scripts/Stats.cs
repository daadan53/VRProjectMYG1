using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stats : MonoBehaviour
{
    private ProfilerRecorder bacthesRecorder;
    private ProfilerRecorder trisRecorder;
    private ProfilerRecorder vertsRecorder;

    public bool canShowStats; // Activer/Désactiver l'affichage
    private string statsTxt;
    private float deltaTime = 0.0f;
    public InputActionAsset inputActions;
    private InputAction menuStats;

    //On record les perf
    private void OnEnable() 
    {
        menuStats = inputActions.FindActionMap("XRI Left Interaction").FindAction("Menu");
        menuStats.Enable();
        menuStats.performed += ToggleMenu;

        bacthesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
        trisRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        vertsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");

        canShowStats = false;
    }

    private void OnDisable() 
    {
        bacthesRecorder.Dispose();
        trisRecorder.Dispose();
        vertsRecorder.Dispose();

        menuStats.performed -= ToggleMenu;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; //Calcule du nb de fps sans prendre en compte les effets de relenti du jeu par ex

        var stringBuilder = new StringBuilder(500);
        stringBuilder.AppendLine($"FPS : {Mathf.Ceil(1.0f / deltaTime)}");
        stringBuilder.AppendLine($"Batches : {bacthesRecorder.LastValue}");
        stringBuilder.AppendLine($"Triangles : {trisRecorder.LastValue}");
        stringBuilder.AppendLine($"Vertices : {vertsRecorder.LastValue}");
        statsTxt = stringBuilder.ToString();
    }

    void OnGUI()
    {
        if (!canShowStats) return;

        GUI.TextArea(new Rect(10,30,300,100), statsTxt);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        canShowStats = !canShowStats;
    }
}
