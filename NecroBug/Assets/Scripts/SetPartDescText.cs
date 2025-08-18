using UnityEngine;
using UnityEngine.UI; 
using TMPro; // Import TextMeshPro namespace

public class SetPartDescText : MonoBehaviour
{
    public TextMeshProUGUI partDescriptionTextObj; 
    public string partDescriptionText;

    public void ChangePartTextOnClick()
    {
        partDescriptionTextObj.text = partDescriptionText;
    }

}
