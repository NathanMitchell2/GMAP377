using UnityEngine;

public class EndScreenButton : MonoBehaviour
{
    [SerializeField] public GameObject firstScreen;
    [SerializeField] public GameObject secondScreen;
    [SerializeField] public GameObject thirdScreen;

    public void FlipFlopOne()
    {
        firstScreen.SetActive(false);
        secondScreen.SetActive(true);
    }

    public void FlipFlopTwo()
    {
        firstScreen.SetActive(false);
        thirdScreen.SetActive(true);
    }

}
