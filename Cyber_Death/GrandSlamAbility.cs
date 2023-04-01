using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoofyGhosts
{
    public class GrandSlamAbility : IAbility
    {
        [SerializeField] private LayerMask whatIsEnemy;

        private SlamAbilityData slamData;
        private CharacterMotor motor;
        private Collider col;
        public GameObject attackRange;
        public GameObject partEff;
        private AudioSource[] aud;

        private void Awake()
        {
            aud = GetComponents<AudioSource>();
        }

        protected override void ActivateAbility()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("Slam", true);
            StartCoroutine("Attack");
            //StartCoroutine(Cast());
        }

        private IEnumerator Attack()
        {
            aud[3].Play();
            attackRange.SetActive(true);
            Quaternion effRot = new Quaternion(-90, 0, 0, 90);
            Instantiate(partEff, attackRange.transform.position, effRot);
            yield return new WaitForSeconds(0.2f);
            attackRange.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("Slam", false);
        }

        /*
        private IEnumerator Cast()
        {
            

         
            List<GameObject> hitEnemies = new List<GameObject>();

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            

                foreach (Collider col in Physics.OverlapSphere(transform.position, slamData.KnockbackRange, whatIsEnemy))
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

           


            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

            yield return new WaitForSeconds(0.3f);
           
        } */
    }
}
