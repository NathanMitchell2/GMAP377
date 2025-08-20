using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string toModelTest = "Model Test";
    [SerializeField] private bool manualSwap = false;
    [SerializeField] private bool resetStuff = true;
    [SerializeField] private int desiredCheckpoint = 0;

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
        else
        {
            CheckpointSystem.Instance.SetCheck(desiredCheckpoint);
        }
        SceneManager.LoadScene(toModelTest);
    }
}