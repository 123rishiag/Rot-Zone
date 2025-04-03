namespace ServiceLocator.Controls
{
    public class InputService
    {
        // Private Variables
        private InputControls inputControls;

        public InputService() => inputControls = new InputControls();
        public void Init() => inputControls.Enable();
        public void Destroy() => inputControls.Disable();
        public void EnableControls() => inputControls.Enable();
        public void DisableControls() => inputControls.Disable();
        public InputControls GetInputControls() => inputControls;
    }
}