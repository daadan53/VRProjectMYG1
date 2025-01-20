using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtFPS;
    [SerializeField] private TextMeshProUGUI txtBatches;
    [SerializeField] private TextMeshProUGUI txtTris;
    [SerializeField] private TextMeshProUGUI txtVerts;

    private ProfilerRecorder bacthesRecorder;
    private ProfilerRecorder trisRecorder;
    private ProfilerRecorder vertsRecorder;

    public bool canShowStats; // Activer/Désactiver l'affichage
    private float deltaTime = 0.0f;
    public InputActionAsset inputActions;
    private InputAction menu;
    [SerializeField] GameObject canvasStats;
    public GameObject menuUiCanvas;

    //On record les perf
    private void OnEnable() 
    {
        menu = inputActions.FindActionMap("XRI Left Interaction").FindAction("Menu");
        menu.Enable();
        menu.started += OnMenuPressed;
        menu.canceled += OnMenuReleased;
        menu.performed += ToggleMenu;

        bacthesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
        trisRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        vertsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");

        canvasStats.SetActive(false);
        menuUiCanvas.SetActive(false);
    }

    private void OnDisable() 
    {
        bacthesRecorder.Dispose();
        trisRecorder.Dispose();
        vertsRecorder.Dispose();

        menu.started -= OnMenuPressed;
        menu.canceled -= OnMenuReleased;
        menu.performed -= ToggleMenu;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; //Calcule du nb de fps sans prendre en compte les effets de relenti du jeu par ex

        if(canvasStats.activeSelf)
        {
            ShowStats();
        }
    }

    void ShowStats()
    {
        txtFPS.text = $"FPS : {Mathf.Ceil(1.0f / deltaTime)}";
        txtBatches.text = $"Batches : {bacthesRecorder.LastValue}";
        txtTris.text = $"Triangles : {trisRecorder.LastValue}";
        txtVerts.text = $"Vertices : {vertsRecorder.LastValue}";
    }

    public void OnMenuPressed(InputAction.CallbackContext context)
    {
        canvasStats.SetActive(true);
    }

    private void OnMenuReleased(InputAction.CallbackContext context)
    {
        canvasStats.SetActive(false);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        menuUiCanvas.SetActive(!menuUiCanvas.activeSelf);
    }
}
