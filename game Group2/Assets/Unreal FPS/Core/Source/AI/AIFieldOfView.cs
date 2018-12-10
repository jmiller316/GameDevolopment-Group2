/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS.AI
{
    /// <summary>
    /// 
    /// </summary>
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    /// <summary>
    /// AI Field Of View class
    /// </summary>
    public class AIFieldOfView : MonoBehaviour
    {
        [SerializeField] private float viewRadius;
        [SerializeField] private float viewAngle;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;

        private List<Transform> visibleTargets = new List<Transform>();


        /// <summary>
        /// Called oce on start
        /// </summary>
        protected virtual void Start()
        {
            StartCoroutine(FindTargetsWithDelay(0.2f));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        protected virtual IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;

                bool isAlive = false;
                if (targetsInViewRadius[i].transform.GetComponent<IHealth>() != null)
                    isAlive = targetsInViewRadius[i].transform.GetComponent<IHealth>().IsAlive;
                else
                    return;

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2 && isAlive)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="globalAngle"></param>
        /// <returns></returns>
        protected virtual ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angleInDegrees"></param>
        /// <param name="angleIsGlobal"></param>
        /// <returns></returns>
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public float ViewRadius
        {
            get
            {
                return viewRadius;
            }
            set
            {
                viewRadius = value;
            }
        }

        public float ViewAngle
        {
            get
            {
                return viewAngle;
            }
            set
            {
                viewAngle = value;
            }
        }

        public List<Transform> VisibleTargets
        {
            get
            {
                return visibleTargets;
            }
            protected set
            {
                visibleTargets = value;
            }
        }

        public LayerMask TargetMask
        {
            get
            {
                return targetMask;
            }

            set
            {
                targetMask = value;
            }
        }

        public LayerMask ObstacleMask
        {
            get
            {
                return obstacleMask;
            }

            set
            {
                obstacleMask = value;
            }
        }
    }
}