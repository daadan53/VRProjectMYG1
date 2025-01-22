using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestAnchor
{
    VirtualVisit virtualVisit;
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
    public IEnumerator TestAnchorWithEnumeratorPasses()
    {
        yield return null;

        for (int i = 0; i < virtualVisit.materials.Length - 1; i++)
        {
            sphereMat = sphere.GetComponent<MeshRenderer>().sharedMaterial;
            Assert.IsNotNull(sphereMat, "Aucun material sur la sphere n'a pas été trouvé");

            virtualVisit.NextWithDelay();
            yield return new WaitForSeconds(1f);

            Assert.AreNotEqual(sphereMat, sphere.GetComponent<MeshRenderer>().sharedMaterial, "La sphere n'a pas chnagé de material");
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < virtualVisit.materials.Length - 1; i++)
        {
            sphereMat = sphere.GetComponent<MeshRenderer>().sharedMaterial;
            Assert.IsNotNull(sphereMat, "Aucun material sur la sphere n'a pas été trouvé");

            virtualVisit.BackWithDelay();
            yield return new WaitForSeconds(1f);

            Assert.AreNotEqual(sphereMat, sphere.GetComponent<MeshRenderer>().sharedMaterial, "La sphere n'a pas chnagé de material");
        }
    }
}
