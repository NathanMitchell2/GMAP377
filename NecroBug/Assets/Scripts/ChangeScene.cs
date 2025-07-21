using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string toModelTest = "Model Test";
    [SerializeField] private bool manualSwap = false;
    [SerializeField] private bool resetStuff = true;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Update()
    {
        if(manualSwap)
            SceneChange();
    }

    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            SceneChange();
        }
    }

    private void SceneChange()
    {
        if (resetStuff)
        {

            InstantAddItem.doCreate = true;
            CheckpointSystem.ResetStatics(resetStuff);
        }
        SceneManager.LoadScene(toModelTest);
    }
}