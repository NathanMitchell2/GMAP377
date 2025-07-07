using UnityEngine;

public class AddFirstCarItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //transform.GetChild(0).GetComponent<PlayerStats>().SetItem(GetComponent<InventoryManager>().GetItem(0));
        //transform.GetChild(0).GetComponent<PlayerHealth>().Initialize(GetComponent<StorageManager>().GetItemList(StorageManager.StorageKey.BotBuilder)[0]);
    }

    /* Procedure:
     * 
     * Add Part to Builder:
     *      Part On Builder
     *          Points to BotPart
     *      BotPart
     *          Points to Part On Builder
     * 
     * Build:
     *      Part on Builder
     *          Points to BotPart
     *      Part on Built
     *          Points to BotPart
     *          Points to BugPart
     *      BotPart
     *          Points to Part On Builder
     *      BugPart
     *          Points to Part On Built
     * 
     *          
     * Cases:
     * 
     * Destroy Case:
     *      BugPart Reports to Part On Built
     *      Part On Built Destroys
     *      BotPart Destroys
     *      
     *      Success
     *      
     * Remove Case:
     *      Builder Reports to Part On Builder
     *      Part On Builder Destroys
     *      
     * Damage And Remove and Craft and Destroy:
     *      ????
     *      
     * 
     */
}
