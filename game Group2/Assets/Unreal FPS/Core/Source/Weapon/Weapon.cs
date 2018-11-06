using UnityEngine;
using System;
using UnityEngine.UI;

namespace UnrealFPS
{
    [Serializable]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private string id = Guid.NewGuid().ToString();
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private string group;
        [SerializeField] private int space;
        [SerializeField] private Sprite image;
        [SerializeField] private GameObject drop;

        /// <summary>
        /// Weapon id
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Weapon name
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;
            }
        }

        /// <summary>
        /// Weapon description
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Weapon texture
        /// </summary>
        public Sprite Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        /// <summary>
        /// Weapon space in inventory
        /// </summary>
        public int Space
        {
            get
            {
                return space;
            }

            set
            {
                space = value;
            }
        }

        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
            }
        }

        /// <summary>
        /// Weapon drop prefab 
        /// </summary>
        public GameObject Drop
        {
            get
            {
                return drop;
            }

            set
            {
                drop = value;
            }
        }
    }
}