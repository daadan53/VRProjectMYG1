using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public bool canShowStats = true; // Activer/Désactiver l'affichage
    private float deltaTime = 0.0f;
    [SerializeField] TextMeshProUGUI txtFPS;
    [SerializeField] TextMeshPro txtBatches;
    [SerializeField] TextMeshPro txtTris;
    [SerializeField] TextMeshPro txtVerts;

    void Start()
    {
        
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void ShowStats()
    {
        if (!canShowStats) return;

        float fps = 1.0f / deltaTime;
        txtFPS.text = $"FPS: {fps:0.}";
        Debug.Log("ca marche ?");

        //txtBatches.text = $"Batches: {UnityEngine.Profiling.Profiler.GetBatchCount()}";
    }
}
