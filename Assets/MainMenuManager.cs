using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame (){
        SceneManager.LoadScene("Scene");
    }
    public void RetryGame (){
        SceneManager.LoadScene("Scene");
    }
    public void QuitGame (){
        Debug.Log("Quit!");
        Application.Quit();

    }
}
