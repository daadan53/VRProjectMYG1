using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualVisit : MonoBehaviour
{
    public Texture2D mainTexture;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject sphere;
    [SerializeField] private Material[] materials;
    Material nextMaterial;
    int STRINGSPACE = 7;
    public GameObject straightArrow;
    public GameObject backArrow;

    private void Start() 
    {
        backArrow.SetActive(false);
    }

    private void ActivateOrNoArrow(Material _currentMaterial)
    {
        if(_currentMaterial == materials[0])
        {
            backArrow.SetActive(false);
            straightArrow.SetActive(true);
        }
        else if(_currentMaterial == materials[materials.Length - 1])
        {
            straightArrow.SetActive(false);
            backArrow.SetActive(true);
        }
        else
        {
            straightArrow.SetActive(true);
            backArrow.SetActive(true);
        }
    }

    //Méthode à la téléportation qui prend le material suivant dans la liste et l'applique à la sphere, puis replace la sphère au niveau du joueur    
    private void Next()
    {
        //On récupère le material suivant dans la liste
        Material nextMaterial = materials[(Array.IndexOf(materials, sphere.GetComponent<MeshRenderer>().sharedMaterial) + 1) % materials.Length];

        sphere.GetComponent<MeshRenderer>().sharedMaterial = nextMaterial;

        Vector3 playerPosition = player.transform.position;
        sphere.transform.position = new Vector3 (sphere.transform.position.x,sphere.transform.position.y, playerPosition.z);

        Material currentMaterial = sphere.GetComponent<MeshRenderer>().sharedMaterial;
        ActivateOrNoArrow(currentMaterial);
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

        Material currentMaterial = sphere.GetComponent<MeshRenderer>().sharedMaterial;
        ActivateOrNoArrow(currentMaterial);
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

    public void TeleportPoint(Button clickedButton)
    {

        //On applique la sphere qui a le meme numéro
        if (clickedButton != null)
        {
            //Récup le nom du bouton
            string buttonName = clickedButton.name;

            string buttonNumber = buttonName.Substring(STRINGSPACE);

            Material newMaterial = materials[Int32.Parse(buttonNumber)];
            sphere.GetComponent<MeshRenderer>().sharedMaterial = newMaterial;

            ActivateOrNoArrow(newMaterial);

        }
        else
        {
            Debug.LogError("Aucun bouton n'a été assigné.");
        }
    }
}
