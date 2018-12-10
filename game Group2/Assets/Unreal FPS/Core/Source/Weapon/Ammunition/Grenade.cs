/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;

namespace UnrealFPS
{
    /// <summary>
    /// Grenade behaviour
    /// </summary>
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private float power;
        [SerializeField] private int damage;
        [SerializeField] private GameObject effect;
        [SerializeField] private float time;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            StartCoroutine(Explosion(time));
        }

        /// <summary>
        /// Explosion grenade after specific time
        /// </summary>
        /// <param name="time">sec</param>
        /// <returns></returns>
        public virtual IEnumerator Explosion(float time)
        {
            yield return new WaitForSeconds(time);
            Vector3 explosionPosition = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
            GameObject _e = Instantiate(effect, explosionPosition, Quaternion.identity);
            ParticleSystem ps = _e.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();
            for (int i = 0; i < colliders.Length; i++)
            {
                IHealth health = colliders[i].gameObject.GetComponent<IHealth>();
                Rigidbody rigidbody = colliders[i].GetComponent<Rigidbody>();

                if (health != null)
                    health.TakeDamage(damage);

                if (rigidbody != null)
                    rigidbody.AddExplosionForce(power, explosionPosition, radius, 3.0f);
            }
            Destroy(gameObject);
            yield break;
        }

        /// <summary>
        /// Explosion radius
        /// </summary>
        /// <value></value>
        public float Radius { get { return radius; } set { radius = value; } }

        /// <summary>
        ///Explosion power
        /// </summary>
        /// <value></value>
        public float Power { get { return power; } set { power = value; } }

        /// <summary>
        /// Explosion damage
        /// </summary>
        /// <value></value>
        public int Damage { get { return damage; } set { damage = value; } }

        /// <summary>
        /// Explosion effect
        /// </summary>
        /// <value></value>
        public GameObject Effect { get { return effect; } set { effect = value; } }

        /// <summary>
        /// Explosion timer
        /// </summary>
        /// <value></value>
        public float Time { get { return time; } set { time = value; } }
    }
}