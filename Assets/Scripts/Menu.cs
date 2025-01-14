using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class Menu : MonoBehaviour
{
    public InputActionAsset inputActions;
    private Canvas menuUiCanvas;
    private InputAction menu;

    void Start()
    {
        menuUiCanvas = GetComponent<Canvas>();
        menuUiCanvas.enabled = false;

        menu = inputActions.FindActionMap("XRI Left Interaction").FindAction("Menu");
        menu.Enable();
        menu.performed += ToggleMenu;
    }

    private void OnDestroy() 
    {
        menu.performed -= ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        menuUiCanvas.enabled = !menuUiCanvas.enabled;
    }
}
