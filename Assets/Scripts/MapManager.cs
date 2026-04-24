using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(FetchRunConfig());
    }

    private IEnumerator FetchRunConfig()
    {
        UnityWebRequest runConfig = UnityWebRequest.Get("http://localhost:5000/run/config");
        yield return runConfig.SendWebRequest();

        if (runConfig.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(runConfig.error);
        }
        else
        {
            RunConfig config = JsonUtility.FromJson<RunConfig>(runConfig.downloadHandler.text);
            Debug.Log(config.monsters.Count);
        }

    }

}
