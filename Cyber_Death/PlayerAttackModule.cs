/******************************************************************
*    Author: Kyle Grenier
*    Contributors: 
*    Date Created: 11/21/2021
*******************************************************************/
using UnityEngine;

namespace GoofyGhosts
{
    public class PlayerAttackModule : WeaponModule
    {
        public int modifier = 0;

        void Awake()
        {
            weaponData = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponUser>().currentWeapon.data;
        }

        public override void OnPurchased()
        {
            weaponData = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponUser>().currentWeapon.data;
            weaponData.attackDamage = new StatUpgrade(weaponData.attackDamage, rank * 1.5f);
            modifier++;
        }

        public void OnPurchased(int value, int mod)
        {
            weaponData = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponUser>().currentWeapon.data;
            weaponData.attackDamage = new StatUpgrade(weaponData.attackDamage, mod);
            modifier += mod;
        }
    }
}