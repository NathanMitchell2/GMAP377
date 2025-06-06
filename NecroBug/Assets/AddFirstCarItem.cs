using UnityEngine;

public class AddFirstCarItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.GetChild(0).GetComponent<PlayerStats>().SetItem(GetComponent<InventoryManager>().GetItem(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
