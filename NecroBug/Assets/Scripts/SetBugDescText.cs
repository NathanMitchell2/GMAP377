using UnityEngine;
using UnityEngine.UI; 
using TMPro; // Import TextMeshPro namespace
using System.Collections.Generic;

public class SetDescText : MonoBehaviour
{
    public TextMeshProUGUI descriptionTextObj; 
    public string descriptionText;
    public string enemyType;
    public int enemyIndex;
    public TextMeshProUGUI partDescriptionTextObj; 
    [SerializeField] private List<GameObject> partButtons = new List<GameObject>();
    public void ChangeTextOnClick()
    {
        if (showDesc()){
            descriptionTextObj.text = descriptionText;
            diplayButtons(enemyIndex);
        }
        else {
            descriptionTextObj.text = "?????";
            diplayButtons(6);
        }
        partDescriptionTextObj.text = "";
    }

    private bool showDesc(){
        if(enemyType == "jetBeetle" && BugopediaEvents.jetBeetleKilled == true){
            return true;
        }
        else if(enemyType == "acidBeetle" && BugopediaEvents.acidBeetleKilled == true){
            return true;
        }
        else if (enemyType == "spider" && BugopediaEvents.spiderKilled == true){
            return true;
        }
        else if (enemyType == "bee" && BugopediaEvents.beeKilled == true){
            return true;
        }
        else if (enemyType == "frenepede" && BugopediaEvents.frenepedeDiscovered == true){
            return true;
        }
        else{
            return false;
        }
    }

    public void diplayButtons(int index){
        foreach (var part in partButtons) {
            part.SetActive(false);
        }
        partButtons[index].SetActive(true);
    }

}
