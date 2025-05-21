using UnityEngine;

public class Sizerandomizer : MonoBehaviour
{
    public Mushroom mushroom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(mushroom != null)
        mushroom.heightMultiplier = Random.Range(1f, 2f);
    }

    
}
