using ServiceLocator.Enemy;
using ServiceLocator.Player;
using ServiceLocator.Projectile;
using ServiceLocator.UI;
using ServiceLocator.Vision;
using ServiceLocator.Wave;
using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Main
{
    public class GameService : MonoBehaviour
    {
        [Header("Camera Variables")]
        public CameraConfig cameraConfig;
        public Camera mainCamera;

        [Header("UI Variables")]
        [SerializeField] public UIView uiCanvas;

        [Header("Pool Panels")]
        public Transform projectilePoolPanel;
        public Transform enemyPoolPanel;

        [Header("Spawn Transform")]
        public Transform spawnTransformPanel;

        [Header("Game Variables")]
        public ProjectileConfig projectileConfig;
        public WeaponConfig weaponConfig;
        public PlayerConfig playerConfig;
        public EnemyConfig enemyConfig;
        public WaveConfig waveConfig;

        // Private Variables
        private GameController gameController;

        private void Start() => gameController = new GameController(this);

        private void OnDestroy() => gameController.Destroy();

        private void Update() => gameController.Update();

        private void LateUpdate() => gameController.LateUpdate();
    }
}