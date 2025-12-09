using System.Collections.Generic;
using StarterAssets;
using UnityEngine;


public class LevelUpMenu : MonoBehaviour
{
    //This menu utilizes different buttons than we were taught in class, 
    //after digging around online and using AI to help me structure and really comprehend how it works
    //I've settled with trying this "dynamic button" system for these random choice buttons.

    public List<UpgradeInfoScript> Upgrades;    //Possible upgrades
    public GameObject levelUpUI;                //The parent for the Level Up UI
    public Transform ChoiceButtonParent;        //The parent of the buttons generated for upgrade choices
    public GameObject buttonPrefab;             //The prefab generated buttons are created from
    public PlayerScript player;                 //The player object for referencing in upgrade
    public ZombieGun gun;                       //The gun  object for referencing in upgrade
    public FirstPersonController movement;      //reference to FirstPersonController to change movespeed

    private int choices = 3;
    private List<UpgradeInfoScript> currentChoices = new List<UpgradeInfoScript>();



    public void showLevelUpMenu()
    {
        //freeze time, and make the cursor usable for interaction
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //choose our three random upgrades from the possible upgrades
        currentChoices.Clear();
        //this pool allows us to remove upgrades after being chosen for selection
        List<UpgradeInfoScript> choicePool = new List<UpgradeInfoScript>(Upgrades); 

        for(int i = 0; i < choices; i++)
        {
            if(choicePool.Count == 0)
            {
                //if there are no upgrades for some reason, break
                break;
            }

            //select a random index to add, add it to our current choices, then remove it from
            //the pool for the next selection
            int randomChoice = Random.Range(0,choicePool.Count);
            currentChoices.Add(choicePool[randomChoice]);
            choicePool.RemoveAt(randomChoice);
        }

        //Now, we begin to create/recreate our buttons for these choices
        //begin by removing the children buttons added before, if any exist
        foreach(Transform buttonChild in ChoiceButtonParent)
        {
            Destroy(buttonChild.gameObject);
        }

        //now, using our upgrades selected in currentChoices, we generate our buttons
        foreach(UpgradeInfoScript upgrade in currentChoices)
        {
            GameObject buttonGameObject = Instantiate(buttonPrefab, ChoiceButtonParent);
            LevelUpButtons button = buttonGameObject.GetComponent<LevelUpButtons>();
            button.setup(upgrade, this);
        }
        //finally activate the menu
        levelUpUI.SetActive(true);

    }

    public void SelectUpgrade(UpgradeInfoScript upgrade)
    {
        //sends the signal to apply the upgrade, then cleans and closes up the menu
        upgrade.applyUpgrade(player, gun, movement);
        CloseLevelUpMenu();
    } 

    public void CloseLevelUpMenu()
    {
        //close the menu, restore time, and lock the cursor to resume play
        levelUpUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


}
