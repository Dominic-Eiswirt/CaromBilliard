using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInGame : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
   public void SettingsButtonControl(GameObject gameObjectToControl)
    {
        if (gameObjectToControl.activeSelf)
        {
            gameObjectToControl.SetActive(false);
        }
        else gameObjectToControl.SetActive(true);
    }

    public void SetAnim()
    {
        if (!anim.GetBool("GearsOn"))
        {
            anim.SetBool("GearsOn", true);            
        }
        else
        {            
            anim.SetBool("GearsOn", false);
        }
    }
}
