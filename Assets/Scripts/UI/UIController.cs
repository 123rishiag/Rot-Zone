using ServiceLocator.Main;

namespace ServiceLocator.UI
{
    public class UIController
    {
        // Private Variables
        private UIView uiView;
        private GameController gameController;

        public UIController(UIView _uiCanvas, GameController _gameController)
        {
            // Setting Variables
            uiView = _uiCanvas.GetComponent<UIView>();
            gameController = _gameController;

            // Adding Listeners
            uiView.pauseMenuResumeButton.onClick.AddListener(gameController.PlayGame);
            uiView.pauseMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

            uiView.gameOverMenuRestartButton.onClick.AddListener(gameController.RestartGame);
            uiView.gameOverMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

            uiView.controlMenuBackButton.onClick.AddListener(gameController.MainMenu);

            uiView.mainMenuPlayButton.onClick.AddListener(gameController.PlayGame);
            uiView.mainMenuControlButton.onClick.AddListener(gameController.ControlMenu);
            uiView.mainMenuQuitButton.onClick.AddListener(gameController.QuitGame);
            uiView.mainMenuMuteButton.onClick.AddListener(gameController.MuteGame);
        }

        public void Destroy()
        {
            // Removing Listeners
            uiView.pauseMenuResumeButton.onClick.RemoveListener(gameController.PlayGame);
            uiView.pauseMenuMainMenuButton.onClick.RemoveListener(gameController.MainMenu);

            uiView.gameOverMenuRestartButton.onClick.RemoveListener(gameController.RestartGame);
            uiView.gameOverMenuMainMenuButton.onClick.RemoveListener(gameController.MainMenu);

            uiView.mainMenuPlayButton.onClick.RemoveListener(gameController.PlayGame);
            uiView.mainMenuControlButton.onClick.RemoveListener(gameController.ControlMenu);
            uiView.mainMenuQuitButton.onClick.RemoveListener(gameController.QuitGame);
            uiView.mainMenuMuteButton.onClick.RemoveListener(gameController.MuteGame);
        }

        public void Reset()
        {
            UpdateHealthText(0);
            UpdateAmmoText(0, 0);
            UpdateEnemiesText(0);
            UpdateWaveText("Wave0");
            UpdateMessageText("");
        }

        // Setters
        public void UpdateHealthText(int _health) => uiView.UpdateHealthText(_health);
        public void UpdateAmmoText(int _currentAmmo, int _totalAmmo) => uiView.UpdateAmmoText(_currentAmmo, _totalAmmo);
        public void UpdateEnemiesText(int _count) => uiView.UpdateEnemiesText(_count);
        public void UpdateWaveText(string _waveString) => uiView.UpdateWaveText(_waveString);
        public void UpdateMessageText(string _message) => uiView.UpdateMessageText(_message);

        public void SetMuteButtonText(bool _isMute) => uiView.SetMuteButtonText(_isMute);
        public void EnablePauseMenuPanel(bool _flag) => uiView.pauseMenuPanel.SetActive(_flag);
        public void EnableGameOverMenuPanel(bool _flag) => uiView.gameOverMenuPanel.SetActive(_flag);
        public void EnableControlMenuPanel(bool _flag) => uiView.controlMenuPanel.SetActive(_flag);
        public void EnableMainMenuPanel(bool _flag) => uiView.mainMenuPanel.SetActive(_flag);

        // Getters
        public bool IsPauseMenuPanelEnabled() => uiView.pauseMenuPanel.activeInHierarchy;
    }
}