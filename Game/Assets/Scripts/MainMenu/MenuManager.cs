using UnityEngine;
using System.Collections;
using System;

public class MenuManager : MonoBehaviour {

    public GameObject currentMenu;

    void Start()
    {
        StartCoroutine(InitializeMenus());
    }

    public void LoadMenu(GameObject menu)
    {
        if (currentMenu != null)
        {
            CloseCurrentMenu();
        }
        currentMenu = menu;
        OpenCurrentMenu();
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    private IEnumerator InitializeMenus()
    {
        foreach (Transform menuChild in transform)
        {
            Animation initializeAnimation = menuChild.GetComponent<Animation>();
            initializeAnimation.Play();
        }   //postavi sve menue u pocetnu poziciju

        yield return new WaitForSeconds(0.1f);  //pricekaj da se sve animacije zavrse
        OpenCurrentMenu();  //otvori prvi menu
    }

    private void OpenCurrentMenu()
    {
        CanvasGroup canvasGroup = currentMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void CloseCurrentMenu()
    {
        CanvasGroup canvasGroup = currentMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
