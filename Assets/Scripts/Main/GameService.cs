using ServiceLocator.Player;
using ServiceLocator.Projectile;
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

        [Header("Pool Panels")]
        public Transform projectilePoolPanel;

        [Header("Game Variables")]
        public ProjectileConfig projectileConfig;
        public WeaponConfig weaponConfig;
        public PlayerConfig playerConfig;

        // Private Variables
        private GameController gameController;

        private void Start() => gameController = new GameController(this);

        private void OnDestroy() => gameController.Destroy();

        private void Update() => gameController.Update();

        private void LateUpdate() => gameController.LateUpdate();
    }
}