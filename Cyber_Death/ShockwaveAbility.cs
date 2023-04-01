/******************************************************************
*    Author: Kyle Grenier
*    Contributors: 
*    Date Created: 11/11/2021
*******************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GoofyGhosts
{
    
    public class ShockwaveAbility : IAbility
    {
        [SerializeField] private LayerMask whatIsEnemy;
        
        private ShockwaveAbilityData shockData;
        private Collider col;
        private SphereCollider sc;
        public GameObject empRange;
        private AudioSource aud;

        private void Awake()
        {
            col = GetComponent<Collider>();
            shockData = base.data as ShockwaveAbilityData;
            sc = GetComponent<SphereCollider>();
            aud = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Activates the ability's unique functionality.
        /// </summary>
        protected override void ActivateAbility()
        {
            StartCoroutine("EMP");
            StartCoroutine("Range");
            //StartCoroutine(Cast());
        }

        private IEnumerator Range()
        {
            empRange.SetActive(true);

            int i = 1;

            while (empRange.transform.localScale.x < 40)
            {
                empRange.transform.localScale = new Vector3(i, i, i);
                yield return new WaitForSeconds(0.00125f);
                i++;
            }

            empRange.SetActive(false);
            empRange.transform.localScale = new Vector3(1, 1, 1);
        }

        private IEnumerator EMP()
        {
            sc.enabled = true;
            aud.Play();
            yield return new WaitForSeconds(0.5f);
            sc.enabled = false;
        }

        /*
        /// <summary>
        /// Performs a sphere cast and knocks back + damages enemies hit.
        /// </summary>
        private IEnumerator Cast()
        {
            
            float currentTime = 0f;
            List<GameObject> hitEnemies = new List<GameObject>();

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            while (currentTime < shockData.ShockDuration)
            {
                currentTime += Time.deltaTime;

                foreach (Collider col in Physics.OverlapSphere(transform.position, shockData.KnockbackRange, whatIsEnemy))
                {
                    if (!hitEnemies.Contains(col.gameObject))
                    {
                        print("Hit " + col.gameObject.name);
                        hitEnemies.Add(col.gameObject);
                        //col.gameObject.GetComponent<CharacterMotor>().AddImpact(-col.transform.forward * dashData.EnemyKnockBack, 1f);

                        //EnemyStateManager manager = col.GetComponent<EnemyStateManager>();

                        //manager.SwapState<EnemyAttackedState>();
                        //manager.OnAttacked(dashData.DashDamage);
                    }
                }

                yield return null;
            }


            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

            yield return new WaitForSeconds(0.3f);
        }
        */
    }
}