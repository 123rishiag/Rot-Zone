using UnityEngine;

public class InputController : MonoBehaviour
{
    // Private Variables
    private InputControls inputControls;

    private void Awake()
    {
        inputControls = new InputControls();
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }

    private void OnDisable()
    {
        inputControls.Disable();
    }

    public InputControls GetInputControls() => inputControls;
}
