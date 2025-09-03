using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class EnemyActivationManager : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 50f;
    public List<EnemyAI> allEnemies = new List<EnemyAI>();

    private static bool s_Warmed;

    private void Awake()
    {
        if (!s_Warmed)
        {
            s_Warmed = true;
            StartCoroutine(WarmAudioOnce());
        }
    }

    private IEnumerator WarmAudioOnce()
    {
      while (!RuntimeManager.HaveAllBanksLoaded)
            yield return null;

        yield return WarmEvent("event:/Bug/bug hurt with scream");
    }

    private IEnumerator WarmEvent(string path)
    {
        var inst = RuntimeManager.CreateInstance(path);

        inst.getDescription(out var desc);
        desc.loadSampleData();

        Transform anchor = player != null ? player :
                           (Camera.main != null ? Camera.main.transform : transform);

        Rigidbody rb = anchor != null ? anchor.GetComponent<Rigidbody>() : null;

        RuntimeManager.AttachInstanceToGameObject(inst, anchor, rb);
        inst.setVolume(0.001f); // effectively silent
        inst.start();
        inst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        inst.release();

        yield return null;
    }

    private void Start()
    {
        allEnemies.AddRange(FindObjectsOfType<EnemyAI>());

        foreach (var enemy in allEnemies)
            enemy.enabled = false;
    }

    private void Update()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(player.position, enemy.transform.position);
            bool shouldBeActive = dist <= activationDistance;

            if (enemy.enabled != shouldBeActive)
            {
                enemy.enabled = shouldBeActive;
                if (shouldBeActive)
                    enemy.ResetAI(); 
            }
        }
    }
}
