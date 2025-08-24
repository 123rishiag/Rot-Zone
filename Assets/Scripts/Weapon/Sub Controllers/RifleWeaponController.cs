using Game.Event;
using Game.Misc;
using Game.Projectile;
using Game.Sound;
using UnityEngine;

namespace Game.Weapon
{
    public class RifleWeaponController : WeaponController
    {
        public RifleWeaponController(WeaponData _weaponData, Transform _parentPanel,
            EventService _eventService, MiscService _miscService, ProjectileService _projectileService)
            : base(_weaponData, _parentPanel, _eventService, _miscService, _projectileService)
        {
        }
        public override void PlayFireSound()
        {
            EventService.OnPlaySoundEffectEvent.Invoke(SoundType.FIRE_RIFLE);
        }
    }
}