using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS
{
    #region Structs
    [Serializable]
    public struct WeaponCompartment
    {
        public Weapon weapon;
        public KeyCode key;
    }

    [Serializable]
    public struct InventoryGroup
    {
        public string name;
        public List<WeaponCompartment> weaponCompartment;
    }
    #endregion

    public class PlayerInventory : MonoBehaviour, IInventory
    {
        [SerializeField] private Transform fpsCamera;
        [SerializeField] List<InventoryGroup> inventoryGroups = new List<InventoryGroup>();

        private bool isSelect;


        private void Update()
        {
            if (Input.anyKeyDown)
                SelectWeaponByKey();

            if (SimpleInputManager.DropWeapon() && GetActiveWeapon() != null)
                DropWeapon(GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon);
        }

        private void SelectWeaponByKey()
        {
            for (int i = 0; i < inventoryGroups.Count; i++)
            {
                for (int j = 0; j < inventoryGroups[i].weaponCompartment.Count; j++)
                {
                    if (Input.GetKeyDown(inventoryGroups[i].weaponCompartment[j].key) && inventoryGroups[i].weaponCompartment[j].weapon != null)
                    {

						StartCoroutine(OnSelectWeapon(inventoryGroups[i].weaponCompartment[j].weapon));
                    }
                }
            }
        }


        public void AddWeapon(Weapon weapon)
        {
            for (int i = 0; i < inventoryGroups.Count; i++)
            {
                if (inventoryGroups[i].name == weapon.Group)
                {

                    for (int j = 0; j < inventoryGroups[i].weaponCompartment.Count; j++)
                    {
                        if (inventoryGroups[i].weaponCompartment[j].weapon == null)
                        {
                            WeaponCompartment weaponCompartment = new WeaponCompartment
                            {
                            weapon = weapon,
                            key = inventoryGroups[i].weaponCompartment[j].key
                            };
                            inventoryGroups[i].weaponCompartment[j] = weaponCompartment;
                            return;
                        }
                    }

                    if (GetActiveWeapon() == null)
                        return;
                        
                    string curGroup = GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon.Group;

                    if (weapon.Group == curGroup)
                    {
                        for (int j = 0; j < inventoryGroups[i].weaponCompartment.Count; j++)
                        {
                            if (inventoryGroups[i].weaponCompartment[j].weapon == GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon)
                            {
                                WeaponCompartment weaponCompartment = new WeaponCompartment
                                {
                                weapon = weapon,
                                key = inventoryGroups[i].weaponCompartment[j].key
                                };
                                inventoryGroups[i].weaponCompartment[j] = weaponCompartment;
                                StartCoroutine(OnElementDrop(GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon));
                                return;
                            }
                        }
                    }
                    else
                    {
                        WeaponCompartment weaponCompartment = new WeaponCompartment
                        {
                            weapon = weapon,
                            key = inventoryGroups[i].weaponCompartment[inventoryGroups[i].weaponCompartment.Count - 1].key
                        };
                        inventoryGroups[i].weaponCompartment[inventoryGroups[i].weaponCompartment.Count - 1] = weaponCompartment;
                        StartCoroutine(OnElementDrop(GetActiveWeapon().GetComponent<WeaponIdentifier>().Weapon));
                        return;
                    }
                }
            }

        }


        public void DropWeapon(Weapon weapon)
        {
            for (int i = 0; i < inventoryGroups.Count; i++)
            {
                if (inventoryGroups[i].name == weapon.Group)
                {
                    for (int j = 0; j < inventoryGroups[i].weaponCompartment.Count; j++)
                    {
                        if (inventoryGroups[i].weaponCompartment[j].weapon == weapon)
                        {
                            WeaponCompartment weaponCompartment = new WeaponCompartment
                            {
                            weapon = null,
                            key = inventoryGroups[i].weaponCompartment[j].key
                            };
                            inventoryGroups[i].weaponCompartment[j] = weaponCompartment;
                            StartCoroutine(OnElementDrop(weapon));
                            return;
                        }
                    }
                }
            }
        }


        public void ActivateWeapon(string id)
        {
            for (int i = 0; i < fpsCamera.childCount; i++)
            {
                if (fpsCamera.GetChild(i).CompareTag("Weapon"))
                {
                    Transform weapon = fpsCamera.GetChild(i);
                    if (weapon.GetComponent<WeaponIdentifier>() != null && weapon.GetComponent<WeaponIdentifier>().Weapon.Id == id)
                        weapon.gameObject.SetActive(true);
                    else
                        weapon.gameObject.SetActive(false);
                }
            }
        }


        public void DeactivateWeapon(string id)
        {
            for (int i = 0; i < fpsCamera.childCount; i++)
            {
                if (fpsCamera.GetChild(i).CompareTag("Weapon"))
                {
                    //saving weapon for easy access
                    Transform weapon = fpsCamera.GetChild(i);
                    if (weapon.GetComponent<WeaponIdentifier>().Weapon.Id == id)
                        weapon.gameObject.SetActive(false);
                }
            }
        }


        public void DeactivateAllWeapons()
        {
            for (int i = 0; i < fpsCamera.childCount; i++)
            {
                if (fpsCamera.GetChild(i).CompareTag("Weapon"))
                    fpsCamera.GetChild(i).gameObject.SetActive(false);
            }
        }


        public void DeactivateActiveWeapon()
        {
            StartCoroutine(DeactivateActiveWeaponHandler());
        }

        public void SelectWeapon(Weapon weapon)
        {
            StartCoroutine(OnSelectWeapon(weapon));
        }


        public Transform GetWeapon(string id)
        {
            for (int i = 0; i < fpsCamera.childCount; i++)
            {
                if (fpsCamera.GetChild(i).CompareTag("Weapon"))
                {
                    //saving weapon for easy access
                    Transform weapon = fpsCamera.GetChild(i);
                    if (weapon.GetComponent<WeaponIdentifier>().Weapon.Id == id)
                        return weapon;
                }
            }
            return null;
        }


        public Transform GetWeapon(int index)
        {
            return (fpsCamera.transform.childCount > index) ? fpsCamera.transform.GetChild(index) : null;
        }


        public Transform GetActiveWeapon()
        {
            for (int i = 0; i < fpsCamera.childCount; i++)
            {
                if (fpsCamera.GetChild(i).CompareTag("Weapon") && fpsCamera.GetChild(i).gameObject.activeSelf)
                {
                    return fpsCamera.GetChild(i);
                }
            }
            return null;
        }


        public IEnumerator OnElementDrop(Weapon weapon)
        {
            isSelect = true;
            yield return new WaitForSeconds(0.3f);
            Transform playerWeapon;
            playerWeapon = GetWeapon(weapon.Id);
            playerWeapon.gameObject.SetActive(false);
            Vector3 pos = fpsCamera.position + fpsCamera.forward * 1;
            GameObject dropWeapon = Instantiate(weapon.Drop, pos, Quaternion.identity);
            if (dropWeapon.GetComponent<Rigidbody>())
                dropWeapon.GetComponent<Rigidbody>().AddForce(fpsCamera.forward * 0.5f, ForceMode.Impulse);
        }


        public IEnumerator OnSelectWeapon(Weapon weapon)
        {
            isSelect = true;
            yield return new WaitForSeconds(0.1f);
            isSelect = false;
            yield return new WaitForSeconds(0.2f);
            ActivateWeapon(weapon.Id);
            yield break;
        }

        public IEnumerator DeactivateActiveWeaponHandler()
        {
            isSelect = true;
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < fpsCamera.childCount; i++)
            {
                if (fpsCamera.GetChild(i).CompareTag("Weapon") && fpsCamera.GetChild(i).gameObject.activeSelf)
                {
                    fpsCamera.GetChild(i).gameObject.SetActive(false);
                }
            }
            yield break;
        }

        public List<InventoryGroup> GetGroups()
        {
            return inventoryGroups;
        }


        public bool IsSelect { get { return isSelect; } }
    }
}