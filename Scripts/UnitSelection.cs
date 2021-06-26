using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    private List<Unit> selectedUnits = new List<Unit>();
    private Vector2 startPos;

    //components
    private Camera cam;
    private Player player;

    private void Awake()
    {
        //get the componets
        cam = Camera.main;
        player = GetComponent<Player>();

    }

    private void Update()
    {
        //check for mouse down
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false); //desabled previously selected units
            selectedUnits = new List<Unit>(); //selecting the new units

            TrySelect(Input.mousePosition);
            startPos = (Input.mousePosition);
        }

        //when mouse up
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();

        }

        //mouse held down
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

    }
    //called when i click on a unit
    void TrySelect (Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit; // shooting the ray down to the screen to see what we have hit

        if(Physics.Raycast(ray,out hit, 100, unitLayerMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if (player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }

        }
    }

    //called when making a selection box
    void UpdateSelectionBox (Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float hight = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(hight));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, hight / 2);

    }

    //makes the box size and makes it go away when you release the mouse
    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach(Unit unit in player.units)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);//turing the world position of workers into a screen position

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y) //seeing if the screen positions sit within the selection box
            {
                //if they do then select them as if i had clicked on them
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }

        }
    }
    //checks to make sure that units that might have died are no longer selected and counted when desiding where other units should be going
    public void RemoveNullUnitsFromSelection()
    {
        for(int x = 0; x<selectedUnits.Count; x++)
        {
            if (selectedUnits[x] == null)
            {
                selectedUnits.RemoveAt(x);
            }
        }
    }

    //toggles if there is a slection ring under the moddle
    void ToggleSelectionVisual (bool selected)
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.ToggleSelectionVisual(selected);
        }
    }

    //returns whether or not we are selecting a unit or units
   public bool HasUnitsSelected ()
    {
        return selectedUnits.Count > 0 ? true : false;
    }

    //returns all the selected units into an array
    public Unit[] getSelectedUnits()
    {
        return selectedUnits.ToArray();
    }
}
