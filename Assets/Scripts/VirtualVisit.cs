using System;
using System.Collections;
using UnityEngine;
public class VirtualVisit : MonoBehaviour
{
    public Texture2D mainTexture;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject sphere;
    [SerializeField] private Material[] materials;
    Material nextMaterial;
    private const float SIZE = 2.4f;
    private bool direction;

    void Start()
    {

    }

    //Méthode à la téléportation qui prend le material suivant dans la liste et l'applique à la sphere, puis replace la sphère au niveau du joueur    
    private void Next()
    {
        //On récupère le material suivant dans la liste
        Material nextMaterial = materials[(Array.IndexOf(materials, sphere.GetComponent<MeshRenderer>().sharedMaterial) + 1) % materials.Length];

        /*Material currentMaterial = sphere.GetComponent<MeshRenderer>().sharedMaterial;
        int currentIndex = System.Array.IndexOf(materials, currentMaterial);
        if (currentIndex == -1)
        {
            Debug.LogWarning("Le matérial actuel n'est pas dans la liste des matérials");
            return;
        }

        nextMaterial = materials[(currentIndex + 1) % materials.Length];*/
        sphere.GetComponent<MeshRenderer>().sharedMaterial = nextMaterial;

        Vector3 playerPosition = player.transform.position;
        sphere.transform.position = new Vector3 (sphere.transform.position.x,sphere.transform.position.y, playerPosition.z);
    }

    private void Back()
    {
        try
        {
            nextMaterial = materials[Array.IndexOf(materials, sphere.GetComponent<MeshRenderer>().sharedMaterial) - 1];
        }
        catch
        {
            nextMaterial = materials[0];
        }

        sphere.GetComponent<MeshRenderer>().sharedMaterial = nextMaterial;

        Vector3 playerPosition = player.transform.position;
        sphere.transform.position = new Vector3 (sphere.transform.position.x,sphere.transform.position.y, playerPosition.z);
    }

    //Pour laisser le temps à unity de verifier la nouvelle position du player
    public void NextWithDelay()
    {
        StartCoroutine(NextAfterTeleport(true));
    }

    public void BackWithDelay()
    {
        StartCoroutine(NextAfterTeleport(false));
    }

    private IEnumerator NextAfterTeleport(bool direction)
    {
        yield return null;

        if(direction)
        Next();

        else
        Back();
    }
}
