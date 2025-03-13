using ServiceLocator.Player;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Main
{
    public class GameService : MonoBehaviour
    {
        [Header("Camera Variables")]
        public CameraConfig cameraConfig;
        public Camera mainCamera;

        [Header("Game Variables")]
        public PlayerConfig playerConfig;
        public WeaponConfig weaponConfig;

        // Private Variables
        private GameController gameController;

        private void Start() => gameController = new GameController(this);

        private void OnDestroy() => gameController.Destroy();

        private void Update() => gameController.Update();

        private void LateUpdate() => gameController.LateUpdate();
    }
}