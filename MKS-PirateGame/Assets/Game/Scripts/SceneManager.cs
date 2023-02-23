using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void LoadMainMenuScene(){
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        Cursor.visible = true;

        LoadScene(0);
    }

    public void LoadGameScene(){
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        Cursor.visible = true;

        LoadScene(1);
    }

    public void LoadScene(string sceneName){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIdx){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIdx);
    }
}
