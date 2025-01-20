using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TestPlayMod
{
    private Menus menuScript;
    private VirtualVisit virtualVisit;
    //Button button;
    Button buttonClicked;
    GameObject sphere;
    Material sphereMat;
    
    [SetUp]
    public void Setup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("Scenes/VirtualVisitScene", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "VirtualVisitScene")
        {
            menuScript = Resources.FindObjectsOfTypeAll<Menus>().FirstOrDefault();
            Assert.IsNotNull(menuScript, "Le script Menus n'a pas été trouvé");

            virtualVisit = GameObject.FindFirstObjectByType<VirtualVisit>();
            Assert.IsNotNull(virtualVisit, "Le script Virtual Visit n'a pas été trouvé");

            sphere = GameObject.Find("Image");
            Assert.IsNotNull(sphere, "La sphere n'a pas été trouvé");

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else
        {
            Assert.Fail("Le nom de la scene est incorrect");
        }
    }
    
    [UnityTest]
    public IEnumerator TestPlayModWithEnumeratorPasses()
    {
        yield return null;

        Assert.IsFalse(menuScript.menuUiCanvas.activeSelf, "Le canvas Menu devrait être désactivé au départ");
        Assert.IsFalse(virtualVisit.backArrow.activeSelf, "La flèche de derrière devrait être dasactivée");

        sphereMat = sphere.GetComponent<MeshRenderer>().sharedMaterial;
        Assert.IsNotNull(sphereMat, "Aucun material sur la sphere n'a pas été trouvé");
        yield return null;

        menuScript.enabled = true;
        Assert.IsTrue(!menuScript.menuUiCanvas.activeSelf, "Le canvas Menu devrait être activé au départ");
        yield return new WaitForSeconds(0.2f);

        //Récup le dernier bouton
        var panel = menuScript.menuUiCanvas.gameObject.transform.Find("Panel").gameObject;
        Assert.IsNotNull(panel, "Le panel 'Panel' n'a pas été trouvé");

        //Récup tous les boutons dans le Panel
        var buttons = panel.GetComponentsInChildren<Button>();
        Assert.IsNotNull(buttons, "Aucun bouton n'a été trouvé dans le panel.");
        Assert.IsTrue(buttons.Length > 0, "Le panel 'Panel' devrait contenir au moins un bouton.");

        /*for(int i=0; i<buttons.Length; i++)
        {
            if(buttons[i].name == "Button 12")
            {
                button = buttons[i];
            }
        }*/

        foreach(Button button in buttons)
        {
            if(button.name == "Button 12")
            {
                buttonClicked = button;
            }
        }

        virtualVisit.TeleportPoint(buttonClicked);
        yield return new WaitForSeconds(1f);

        Assert.IsFalse(virtualVisit.straightArrow.activeSelf, "La flèche de devant devrait être désactivée");
        //On compare le nouveau material de la sphère à l'ancien 
        Assert.AreNotEqual(sphereMat, sphere.GetComponent<MeshRenderer>().sharedMaterial, "Le material de la sphere n'a pas changé");

        yield return new WaitForSeconds(2f);
    }
}
