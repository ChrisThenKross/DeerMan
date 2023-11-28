using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStage2Boss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        callScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callScene()
    {
        Invoke(nameof(SceneCalling), 30.0f);
    }

    public void SceneCalling()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("bf2 arena");
    }
}