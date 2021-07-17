using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public string StartButtonScene = "SampleScene";
    public string CreditButtonScene = "Credits";
    public string BackButtonScene = "Title Screen";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene(StartButtonScene);
    }

    public void CreditButton()
    {
        SceneManager.LoadScene(CreditButtonScene);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        SceneManager.LoadScene(BackButtonScene);
    }
}
