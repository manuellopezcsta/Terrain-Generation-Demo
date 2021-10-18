using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quitted");
    }
}
