using UnityEngine;
using System.Collections.Generic;

public class TutorialPanelManager : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointPanelPair
    {
        public Transform checkpoint;
        public GameObject tutorialPanel;
    }

    public List<CheckpointPanelPair> panelPairs = new List<CheckpointPanelPair>();

    private Dictionary<Transform, GameObject> panelMap = new Dictionary<Transform, GameObject>();

    private void Start()
    {
        foreach (var pair in panelPairs)
        {
            if (pair.checkpoint && pair.tutorialPanel)
                panelMap[pair.checkpoint] = pair.tutorialPanel;
        }
    }

    public void ShowPanelForCheckpoint(Transform checkpoint)
    {
        if (panelMap.TryGetValue(checkpoint, out GameObject panel))
        {
            panel.SetActive(true);
        }
    }
} 