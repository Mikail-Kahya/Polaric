using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OpenSubMenu(GameObject subMenu)
    {
        gameObject.SetActive(false);
        subMenu.SetActive(true);
    }

    public void CloseSubMenu(GameObject subMenu)
    {
        subMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
