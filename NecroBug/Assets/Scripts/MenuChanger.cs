using System.Collections.Generic;
using UnityEngine;

public class MenuChanger : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus = new List<GameObject>();

    public void SetMenu(int index)
    {
        Off();

        menus[index].SetActive(true);
    }
    public void Off()
    {
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }
    }
}
