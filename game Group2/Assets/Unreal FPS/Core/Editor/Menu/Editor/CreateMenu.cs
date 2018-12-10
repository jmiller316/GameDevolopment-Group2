/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;
using UnrealFPS;
using UnrealFPS.Utility;
using UnrealFPS.AI;

namespace UnrealFPS.Editor
{
    public static class CreateMenu
    {
        private static GameObject player = Resources.Load("Prefab/Player") as GameObject;
        private static GameObject physicsBullet = Resources.Load("Prefab/PhysicsBullet") as GameObject;
        private static GameObject lootWeapon = Resources.Load("Prefab/LootWeapon") as GameObject;
        private static GameObject ai = Resources.Load("Prefab/AI") as GameObject;


        [MenuItem("Unreal FPS/Create/Player", false, 21)]
        public static void CreatePlayer()
        {
            Object.Instantiate(player, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Unreal FPS/Create/Weapon", false, 22)]
        public static void CreateWeapon()
        {
            ScriptableObjectUtility.CreateAsset<Weapon>();
        }

        [MenuItem("Unreal FPS/Create/AI", false, 24)]
        public static void CreateAI()
        {
            Object.Instantiate(ai, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Unreal FPS/Create/Bullet/RayBullet", false, 71)]
        public static void CreateRayBullet()
        {
            ScriptableObjectUtility.CreateAsset<RayBullet>();
        }

        [MenuItem("Unreal FPS/Create/Bullet/PhysicsBullet", false, 72)]
        public static void CreatePhysicsBullet()
        {
            Object.Instantiate(physicsBullet, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Unreal FPS/Create/Loot/Weapon", false, 73)]
        public static void CreateLootWeapon()
        {
            Object.Instantiate(lootWeapon, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Unreal FPS/Create/Spawn/Player Area", false, 75)]
		public static void CreatePlayerSpawnArea()
		{
			GameObject spawnArea = new GameObject();
			spawnArea.name = "Player Spawn Area";
			spawnArea.AddComponent<SpawnManager>();
		}

		[MenuItem("Unreal FPS/Create/Spawn/AI Area", false, 76)]
		public static void CreateAISpawnArea()
		{
			GameObject spawnArea = new GameObject();
			spawnArea.name = "AI Spawn Area";
			spawnArea.AddComponent<AISpawnManager>();
		}
    }
}