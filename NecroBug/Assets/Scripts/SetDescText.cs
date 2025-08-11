using UnityEngine;
using UnityEngine.UI; 
using TMPro; // Import TextMeshPro namespace

public class SetDescText : MonoBehaviour
{
    public TextMeshProUGUI descriptionTextObj; 
    public string descriptionText;
    public string enemyType;

    public TextMeshProUGUI dropsTextObj;
    public string dropsText;
    public void ChangeTextOnClick()
    {
        if (showDesc()){
            descriptionTextObj.text = descriptionText;
            dropsTextObj.text = "Drops:\n"+dropsText;
        }
        else {
            descriptionTextObj.text = "?????";
            dropsTextObj.text = "Drops:\n??????";
        }
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
        else if (enemyType== "bee" && BugopediaEvents.beeKilled == true){
            return true;
        }
        else{
            return false;
        }
    }

}
