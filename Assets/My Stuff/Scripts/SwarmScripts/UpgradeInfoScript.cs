using System.IO;
using StarterAssets;
using UnityEngine;

//This script is a Scriptable object, from what ive gathered online this are a good way to create upgrades on the fly
[CreateAssetMenu(fileName = "NewUpgrade", menuName ="Swarm/Upgrade")]
public class UpgradeInfoScript :ScriptableObject
{
    //This script contains the ability to upgrade a player, aswell as define new upgrades
    public string UpgradeName;  //The name of the upgrade
    public Sprite Icon;         //Its sprite/icon
    public UpgradeType type;    //The type the upgrade falls under
    public float amount;        //How much it upgrades


    public void applyUpgrade(PlayerScript player, ZombieGun gun, FirstPersonController movement)
    {
        //switch case based around what type of upgrade we are given
        switch (type)
        {
            case UpgradeType.Health:
                player.maxHealth += amount;
                player.heal(50);
                break;

            case UpgradeType.Speed:
                movement.MoveSpeed += amount;
                break;
                
            case UpgradeType.Damage:
                player.damageModifer += amount;
                break;

            case UpgradeType.FireRate:
                gun.roundsPerMinute += amount*1000;
                gun.fireDelay = 60f / gun.roundsPerMinute;
                break;

        }
    }

}

//The types of upgrades, easily expandable to hold whatever you desire
public enum UpgradeType { Health, Speed, Damage, FireRate }


