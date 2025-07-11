using Game.Event;
using Game.Sound;
using Game.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerWeaponController
    {
        // Private Variables
        private PlayerController playerController;

        private WeaponType currentWeaponType;
        private Dictionary<WeaponType, WeaponController> weapons;
        private WeaponTransform currentWeaponTransform;

        // Private Services
        private EventService eventService;
        private WeaponService weaponService;

        public PlayerWeaponController(PlayerController _playerController,
            EventService _eventService, WeaponService _weaponService)
        {
            // Setting Variables
            playerController = _playerController;

            // Setting Services
            eventService = _eventService;
            weaponService = _weaponService;

            // Setting Variables
            currentWeaponType = WeaponType.NONE;
            weapons = new Dictionary<WeaponType, WeaponController>();
            SetCurrentWeaponSetting();
            CreateWeapons();
        }

        private void CreateWeapons()
        {
            foreach (WeaponIKData weaponIKData in playerController.GetView().GetWeaponIKDatas())
            {
                Transform parentTransform =
                    GetWeaponIKData(weaponIKData.weaponType).weaponTransform.weaponHolder;
                WeaponController weapon = weaponService.CreateWeapon(weaponIKData.weaponType, parentTransform);
                weapons[weaponIKData.weaponType] = weapon;

                AttachWeaponToRightHand(weaponIKData.weaponType);
            }
            SwitchOffWeapons();
        }
        private void AttachWeaponToRightHand(WeaponType _weaponType)
        {
            WeaponIKData weaponIKData = GetWeaponIKData(_weaponType);
            Transform rightHand_TargetTransform = weaponIKData.weaponTransform.rightHand_TargetTransform;

            weapons[_weaponType].SetTransform(rightHand_TargetTransform);
        }
        public void EquipWeapon(WeaponType _weaponType)
        {
            if (playerController.GetMovementStateMachine().GetCurrentState() == PlayerMovementState.FALL)
            {
                currentWeaponType = WeaponType.NONE;
            }
            else if (currentWeaponType == _weaponType)
            {
                return;
            }
            else
            {
                currentWeaponType = _weaponType;
            }

            // Resetting Movement State and Action State  while equipment changes,
            // as to avoid unwanted behaviours
            playerController.GetMovementStateMachine().ChangeState(PlayerMovementState.IDLE);
            playerController.GetActionStateMachine().ChangeState(PlayerActionState.NONE);

            SwitchOffWeapons();

            if (currentWeaponType != WeaponType.NONE)
            {
                weapons[currentWeaponType].EnableWeapon(playerController.GetModel().AimLayer);

                WeaponIKData weaponIKData = GetWeaponIKData(currentWeaponType);
                currentWeaponTransform = weaponIKData.weaponTransform;

                AttachLeftHandToWeapon(currentWeaponType);
                SetRightHandWeaponOffset();

                eventService.OnPlaySoundEffectEvent.Invoke(SoundType.WEAPON_EQUIP);
            }

            SetCurrentWeaponSetting();
            playerController.UpdateAmmoUI();
        }

        private void SwitchOffWeapons()
        {
            ReloadComplete();
            foreach (WeaponIKData weaponIKData in playerController.GetView().GetWeaponIKDatas())
            {
                weapons[weaponIKData.weaponType].DisableWeapon();
            }
        }

        public void SetRightHandWeaponOffset()
        {
            playerController.GetView().GetRightHandAimConstraint().data.offset = currentWeaponTransform.rightHand_TargetOffset;
        }

        private void AttachLeftHandToWeapon(WeaponType _weaponType)
        {
            Transform currentLeftHand_Target = currentWeaponTransform.leftHand_TargetTransform;
            Transform currentLeftHand_Hint = currentWeaponTransform.leftHand_HintTransform;

            SetLocalTransform(playerController.GetView().GetLeftHandIK().data.target,
                currentLeftHand_Target);
            SetLocalTransform(playerController.GetView().GetLeftHandIK().data.hint,
                currentLeftHand_Hint);
        }
        public void ReloadWeapon()
        {
            weapons[currentWeaponType].ReloadWeapon();
            playerController.UpdateAmmoUI();
        }

        public void SetAmmo(WeaponType _weaponType, int _ammoAmount)
        {
            weapons[_weaponType].SetAmmo(_ammoAmount);
        }

        public void ReloadComplete() => playerController.IsReloading = false;

        public void FireWeapon()
        {
            ReloadComplete();
            playerController.ApplyKickback();
            weapons[currentWeaponType].FireWeapon();

            // To Stop Constant Firing if the weapon is Single Type
            if (weapons[currentWeaponType].GetModel().WeaponFireType == WeaponFireType.SINGLE)
            {
                playerController.IsFiring = false;
            }
            playerController.UpdateAmmoUI();
        }

        // Setters
        private void SetCurrentWeaponSetting()
        {
            playerController.GetAnimationController().SetAnimationLayer(currentWeaponType);
            playerController.GetAnimationController().SetIKWeight(currentWeaponType);
        }
        private void SetLocalTransform(Transform _targetTransform, Transform _sourceTransform)
        {
            _targetTransform.localPosition = _sourceTransform.localPosition;
            _targetTransform.localRotation = _sourceTransform.localRotation;
            _targetTransform.localScale = _sourceTransform.localScale;
        }
        // Getters
        public WeaponType GetCurrentWeaponType() => currentWeaponType;
        public WeaponController GetCurrentWeapon() => weapons[currentWeaponType];
        public WeaponTransform GetCurrentWeaponTransform() =>
            GetWeaponIKData(currentWeaponType).weaponTransform;
        private WeaponIKData GetWeaponIKData(WeaponType _weaponType) =>
            Array.Find(playerController.GetView().GetWeaponIKDatas(), w => w.weaponType == _weaponType);
    }
}