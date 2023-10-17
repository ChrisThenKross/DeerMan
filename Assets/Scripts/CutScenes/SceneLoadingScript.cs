using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
