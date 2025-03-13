using ServiceLocator.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerWeaponController
    {
        // IK Parameters
        private float defaultIKWeight = 0.0f;
        private float pistolIKWeight = 1.0f;
        private float rifleIKWeight = 1.0f;
        private float shotgunIKWeight = 1.0f;

        // Private Variables
        private PlayerController playerController;

        private WeaponType currentWeaponType;
        private Dictionary<WeaponType, WeaponController> weapons;
        private WeaponTransform currentWeaponTransform;

        // Private Services
        private WeaponService weaponService;

        public PlayerWeaponController(PlayerController _playerController,
            WeaponService _weaponService)
        {
            // Setting Variables
            playerController = _playerController;

            // Setting Services
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
            SwitchOffWeapons();
            currentWeaponType = _weaponType;

            if (currentWeaponType != WeaponType.NONE)
            {
                weapons[_weaponType].EnableWeapon();

                WeaponIKData weaponIKData = GetWeaponIKData(_weaponType);
                currentWeaponTransform = weaponIKData.weaponTransform;

                AttachLeftHandToWeapon(_weaponType);
            }

            SetCurrentWeaponSetting();
        }
        private void SwitchOffWeapons()
        {
            foreach (WeaponIKData weaponIKData in playerController.GetView().GetWeaponIKDatas())
            {
                weapons[weaponIKData.weaponType].DisableWeapon();
            }
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

        // Setters
        private void SetCurrentWeaponSetting()
        {
            playerController.GetAnimationController().SetAnimationLayer(currentWeaponType);
            SetIKWeight();
        }
        private void SetIKWeight()
        {
            float weight = GetIKWeight();
            playerController.GetView().GetLeftHandIK().weight = weight;
            playerController.GetView().GetRightHandAimConstraint().weight = weight;
        }
        private void SetLocalTransform(Transform _targetTransform, Transform _sourceTransform)
        {
            _targetTransform.localPosition = _sourceTransform.localPosition;
            _targetTransform.localRotation = _sourceTransform.localRotation;
            _targetTransform.localScale = _sourceTransform.localScale;
        }
        // Getters
        public WeaponType GetCurrentWeapon() => currentWeaponType;
        private WeaponIKData GetWeaponIKData(WeaponType _weaponType) =>
            Array.Find(playerController.GetView().GetWeaponIKDatas(), w => w.weaponType == _weaponType);
        private float GetIKWeight()
        {
            switch (currentWeaponType)
            {
                case WeaponType.PISTOL:
                    return pistolIKWeight;
                case WeaponType.RIFLE:
                    return rifleIKWeight;
                case WeaponType.SHOTGUN:
                    return shotgunIKWeight;
                case WeaponType.NONE:
                default:
                    return defaultIKWeight;
            }
        }
    }
}