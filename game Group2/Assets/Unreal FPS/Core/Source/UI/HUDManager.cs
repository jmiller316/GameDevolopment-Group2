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
using UnrealFPS.Utility;

namespace UnrealFPS.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Scrollbar healthSlider;
        [SerializeField] private Text healthCount;
        [SerializeField] private Text bulletCount;
        [SerializeField] private Text clipCount;
        [SerializeField] private Text weaponName;
        [SerializeField] private Sprite weaponImage;
        [SerializeField] private Transform root;
        [SerializeField] private Text message;

        private int lastHealth;
        private IInventory inventory;
        private PlayerHealth health;

        private void Start()
        {
            inventory = player.GetComponent<IInventory>();
            health = player.GetComponent<PlayerHealth>();
            lastHealth = health.Health;
        }

        private void Update()
        {
            HUDObserver();
        }

        /// <summary>
        /// Processing HUD components
        /// </summary>
        private void HUDObserver()
        {
            if (health.Health != lastHealth)
            {
                float playerHealth = health.Health;
                if (healthCount != null)
                    healthCount.text = playerHealth.GetPersent(health.MaxHealth).ToString("#") + "%";
                if (healthSlider != null)
                    healthSlider.size = playerHealth.GetPersent(health.MaxHealth) / 100;
                lastHealth = health.Health;
            }

            if (inventory.GetActiveWeapon() != null)
            {
                if (inventory.GetActiveWeapon().GetComponent<WeaponReloadSystem>() != null)
                {
                    int bcount = inventory.GetActiveWeapon().GetComponent<WeaponReloadSystem>().BulletCount;
                    int cCount = inventory.GetActiveWeapon().GetComponent<WeaponReloadSystem>().ClipCount;
                    if (bulletCount != null && (bulletCount.text != bcount.ToString()))
                    {
                        bulletCount.text = bcount.ToString();
                    }
                    if (clipCount != null && (clipCount.text != cCount.ToString()))
                    {
                        clipCount.text = cCount.ToString();
                    }
                }
                if (weaponName != null && (weaponName.text != inventory.GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon.DisplayName))
                {
                    weaponName.text = inventory.GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon.DisplayName;
                }
                if (weaponImage != null && (weaponImage != inventory.GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon.Image))
                {
                    weaponImage = inventory.GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon.Image;
                }

            }
            else
            {
                bulletCount.text = "--";
                clipCount.text = "----";
                weaponName.text = "Hands";
            }
        }

        /// <summary>
        /// Send message on HUD
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string text)
        {
            if (root != null)
            {
                root.gameObject.SetActive(true);
                message.text = text;
            }
        }

        /// <summary>
        /// Hide message from HUD
        /// </summary>
        public void HideMessage()
        {
            if (root != null)
            {
                root.gameObject.SetActive(false);
                message.text = "";
            }
        }

        /// <summary>
        /// Health count
        /// </summary>
        public Text HealthCount
        {
            get
            {
                return healthCount;
            }

            set
            {
                healthCount = value;
            }
        }

        /// <summary>
        /// Bullet Count
        /// </summary>
        public Text BulletCount
        {
            get
            {
                return bulletCount;
            }

            set
            {
                bulletCount = value;
            }
        }

        /// <summary>
        /// Clip Count
        /// </summary>
        public Text ClipCount
        {
            get
            {
                return clipCount;
            }

            set
            {
                clipCount = value;
            }
        }

        /// <summary>
        /// Weapon Name
        /// </summary>
        public Text WeaponName
        {
            get
            {
                return weaponName;
            }

            set
            {
                weaponName = value;
            }
        }

        /// <summary>
        /// Weapon Image
        /// </summary>
        public Sprite WeaponImage
        {
            get
            {
                return weaponImage;
            }

            set
            {
                weaponImage = value;
            }
        }
    }
}