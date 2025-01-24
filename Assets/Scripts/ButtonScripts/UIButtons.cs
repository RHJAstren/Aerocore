using UnityEngine;

public class UIButtons : Button
{
    public GameObject[] Panels;

    public override void HandleButton()
    {
        base.HandleButton();
        Panels[0].SetActive(true);
        Panels[1].SetActive(false);
    }
}
