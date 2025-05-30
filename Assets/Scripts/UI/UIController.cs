using Game.Event;
using Game.Main;
using Game.Wave;

namespace Game.UI
{
    public class UIController
    {
        // Private Variables
        private UIView uiView;
        private GameController gameController;

        // Private Services
        private EventService eventService;

        public UIController(UIView _uiCanvas, GameController _gameController, EventService _eventService)
        {
            // Setting Variables
            uiView = _uiCanvas.GetComponent<UIView>();
            gameController = _gameController;

            // Setting Services
            eventService = _eventService;

            // Adding Listeners
            eventService.OnPlayerHealthUIUpdateEvent.AddListener(UpdateHealthText);
            eventService.OnPlayerAmmoUIUpdateEvent.AddListener(UpdateAmmoText);
            eventService.OnEnemyCountUIUpdateEvent.AddListener(UpdateEnemiesText);
            eventService.OnWaveUIUpdateEvent.AddListener(UpdateWaveText);
            eventService.OnLoadTextUIUpdateEvent.AddListener(UpdateLoadText);

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
            eventService.OnPlayerHealthUIUpdateEvent.RemoveListener(UpdateHealthText);
            eventService.OnPlayerAmmoUIUpdateEvent.RemoveListener(UpdateAmmoText);
            eventService.OnEnemyCountUIUpdateEvent.RemoveListener(UpdateEnemiesText);
            eventService.OnWaveUIUpdateEvent.RemoveListener(UpdateWaveText);
            eventService.OnLoadTextUIUpdateEvent.RemoveListener(UpdateLoadText);

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
            UpdateWaveText(WaveType.WAVE1);
            UpdateLoadText(false);
        }

        // Setters
        public void UpdateHealthText(int _health) => uiView.UpdateHealthText(_health);
        public void UpdateAmmoText(int _currentAmmo, int _totalAmmo) => uiView.UpdateAmmoText(_currentAmmo, _totalAmmo);
        public void UpdateEnemiesText(int _count) => uiView.UpdateEnemiesText(_count);
        public void UpdateWaveText(WaveType _waveType) => uiView.UpdateWaveText(_waveType);
        public void UpdateLoadText(bool _flag) => uiView.UpdateLoadText(_flag);

        public void SetMuteButtonText(bool _isMute) => uiView.SetMuteButtonText(_isMute);
        public void EnablePauseMenuPanel(bool _flag) => uiView.pauseMenuPanel.SetActive(_flag);
        public void EnableGameOverMenuPanel(bool _flag) => uiView.gameOverMenuPanel.SetActive(_flag);
        public void EnableControlMenuPanel(bool _flag) => uiView.controlMenuPanel.SetActive(_flag);
        public void EnableMainMenuPanel(bool _flag) => uiView.mainMenuPanel.SetActive(_flag);

        // Getters
        public bool IsPauseMenuPanelEnabled() => uiView.pauseMenuPanel.activeInHierarchy;
    }
}