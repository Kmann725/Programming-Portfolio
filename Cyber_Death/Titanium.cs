using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoofyGhosts
{
    public class Titanium : IAbility
    {
        [SerializeField] Armor playerArmor;
        //[SerializeField] ArmorData playerArmorData;
        bool cooldownBool;
        private float currentArmor;
        public GameObject player;
        public Material silver;
        public Material body1;
        public Material body2;
        private float increase;
        private AudioSource[] aud;

        private void Awake()
        {
            aud = GetComponents<AudioSource>();
        }

        protected override void ActivateAbility()
        {
            StartCoroutine("Armored");
        }

        private IEnumerator Armored()
        {
            aud[1].Play();
            increase = 19 - playerArmor.CurrentArmor.GetStat();
            currentArmor = playerArmor.CurrentArmor.GetStat();
            Material[] mats = player.GetComponent<SkinnedMeshRenderer>().materials;
            mats[2] = silver;
            mats[3] = silver;
            player.GetComponent<SkinnedMeshRenderer>().materials = mats;
            playerArmor.CurrentArmor = new StatUpgrade(playerArmor.CurrentArmor, increase);
            yield return new WaitForSeconds(8f);
            aud[2].Play();
            mats[2] = body2;
            mats[3] = body1;
            player.GetComponent<SkinnedMeshRenderer>().materials = mats;
            playerArmor.CurrentArmor = new StatUpgrade(playerArmor.CurrentArmor, -increase);
        }
    }
}