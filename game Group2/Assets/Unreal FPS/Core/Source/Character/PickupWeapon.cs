/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.UI;

namespace UnrealFPS
{
    [System.Serializable]
    public class PickupWeapon
    {
        [SerializeField] private float radius;
        [SerializeField] private bool destroyOnPickUp;

        [Header("UI")]
        [SerializeField] private Transform panel;
        [SerializeField] private string prefixMessage;
        [SerializeField] private Text textMessage;

        private Transform player;
        private PlayerInventory inventory;
        private float lastDistance;
        private WeaponIdentifier weaponID;

        public void Init(Transform player, PlayerInventory inventory)
        {
            this.player = player;
            this.inventory = inventory;
            lastDistance = radius;
        }

        /// <summary>
        /// Pickup weapon handler
        /// </summary>
        public void Handler()
        {
            Collider[] collider = Physics.OverlapSphere(player.position, radius);
            for (int i = 0; i < collider.Length; i++)
            {
                if (collider[i].CompareTag("Weapon"))
                {
                    Transform weapon = collider[i].transform;
                    lastDistance = Vector3.Distance(player.position, weapon.position);
                    if (Vector3.Distance(player.position, weapon.position) <= lastDistance)
                    {
                        weaponID = weapon.GetComponent<WeaponIdentifier>();
                    }
                }
            }
            if (weaponID != null)
            {
                //ShowWeaponInfo(prefixMessage + weaponID.Weapon.DisplayName);
                if (SimpleInputManager.GetInteract())
                {
                    inventory.AddWeapon(weaponID.Weapon);
                    inventory.SelectWeapon(weaponID.Weapon);
                    if (destroyOnPickUp) { Object.Destroy(weaponID.gameObject); }
                    //HideWeaponInfo();
                }
            }

            if (lastDistance > radius)
            {
                //HideWeaponInfo();
                weaponID = null;
            }
        }

        private void ShowWeaponInfo(string message)
        {
            panel.gameObject.SetActive(true);
            textMessage.text = message;
        }

        private void HideWeaponInfo()
        {
            panel.gameObject.SetActive(false);
            textMessage.text = "";
        }
    }
}