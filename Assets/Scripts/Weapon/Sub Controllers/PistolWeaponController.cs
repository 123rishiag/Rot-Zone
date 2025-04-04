using ServiceLocator.Event;
using ServiceLocator.Projectile;
using ServiceLocator.Sound;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class PistolWeaponController : WeaponController
    {
        public PistolWeaponController(WeaponData _weaponData, Transform _parentPanel,
            EventService _eventService, ProjectileService _projectileService)
            : base(_weaponData, _parentPanel, _eventService, _projectileService)
        {
        }
        public override void PlayFireSound()
        {
            eventService.OnPlaySoundEffectEvent.Invoke(SoundType.FIRE_PISTOL);
        }
    }
}