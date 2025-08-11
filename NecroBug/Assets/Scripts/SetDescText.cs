using UnityEngine;
using UnityEngine.UI; 
using TMPro; // Import TextMeshPro namespace

public class SetDescText : MonoBehaviour
{
    public TextMeshProUGUI descriptionTextObj; 
    public string descriptionText;

    public TextMeshProUGUI dropsTextObj;
    public string dropsText;
    public void ChangeTextOnClick()
    {
        descriptionTextObj.text = descriptionText;
        dropsTextObj.text = "Drops:\n"+dropsText;
    }

}
