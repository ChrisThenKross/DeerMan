using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToStage3Boss : MonoBehaviour
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
        Invoke(nameof(SceneCalling), 32.2f);
    }

    public void SceneCalling()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Boss 3 (Train)");
    }
}
