using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceSourceUI : MonoBehaviour
{
    //setting up panel to appear when the mouse hovers over a resources
    public GameObject popupPanel;
    public TextMeshProUGUI resourceQuatityText;
    public ResourceSource resource;

    private void OnMouseEnter()
    {
        popupPanel.SetActive(true);
    }

    private void OnMouseExit()
    {
        popupPanel.SetActive(false);
    }

    public void OnResourceQuantityChange()
    {
        resourceQuatityText.text = resource.quantity.ToString();
    }
}


