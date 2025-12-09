using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpButtons : MonoBehaviour
{
    //This is the basic functionality for the dynamic buttons
    //containing a setup and onclick functions
    //this allows us to dynamically use any type of upgrade button within these function to apply their desired results
    //within levelupmenu.cs

    public UpgradeInfoScript upgrade;
    public LevelUpMenu MenuManager;
    public Image buttonImage;
    public TextMeshProUGUI upgradeDescriptionText;

    public void setup(UpgradeInfoScript upgrade, LevelUpMenu menu)
    {
        //assign our public fields dynamically
        this.upgrade = upgrade;
        this.MenuManager = menu;

        //Assign our icons/descriptions for the button
        if(buttonImage != null)
        {
            buttonImage.sprite = upgrade.Icon;
        }
        if(upgradeDescriptionText != null)
        {
            upgradeDescriptionText.text = upgrade.UpgradeName;
        }

        //clear all old listeners to create our new ones
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);


    }

    public void OnClick()
    {
        //call into levelupmenu.cs that the user selected the clicked upgrade
        MenuManager.SelectUpgrade(upgrade);
    }

    
}
