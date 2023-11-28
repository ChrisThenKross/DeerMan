using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Transition : MonoBehaviour
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
        Invoke(nameof(SceneCalling), 31.0f);
    }

    public void SceneCalling()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map Gen 2");
    }
}
