using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public GameObject selectionMarkerPrefab;
    public LayerMask layerMask;

    //components
    private UnitSelection unitSelection;
    private Camera cam;

    private void Awake()
    {
        //get components
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;

    }

    private void Update()
    {
        //if the right mouse button is being clicked down
        if(Input.GetMouseButtonDown(1) && unitSelection.HasUnitsSelected())
        {
            //shoot a raycast
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //cache the selected units in an array
            Unit[] selectedUnits = unitSelection.getSelectedUnits();

            if(Physics.Raycast(ray,out hit, 100, layerMask))
            {
                unitSelection.RemoveNullUnitsFromSelection();

                //did we click on the ground to move somthing
                if (hit.collider.CompareTag("Ground"))
                {
                    UnitsMoveToPosition(hit.point, selectedUnits);
                    CreateSelectionMarker(hit.point,false);//creating a selection marker on the possition that was clicked
                }

                //did we click on a resrouce
                else if (hit.collider.CompareTag("Resource"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);
                    CreateSelectionMarker(hit.collider.transform.position, true); //changing the selection marker to make it bigger
                }

                //did we click on a unit 
                else if (hit.collider.CompareTag("Unit"))
                {
                    Unit enemy = hit.collider.gameObject.GetComponent<Unit>();
                    //is this my unit
                    if (!Player.me.IsMyUnit(enemy))
                    {
                        //then attack
                        UnitsAttackEnemy(enemy, selectedUnits);
                        CreateSelectionMarker(enemy.transform.position, false);
                        
                    }

                }


            }
        }
    }

    //runs through all of the selected workers, and makes them move to the given possition
    void UnitsMoveToPosition(Vector3 movePos, Unit[] units)
    {
        Vector3[] destinations = UnitMover.GetUnitGroupDestinations(movePos, units.Length, 2);

        for(int x = 0; x < units.Length; x++)
        {
            units[x].MoveToPosition(destinations[x]);
        }
    }

    //function to send units to a resource so that the will collect
    void UnitsGatherResource(ResourceSource resource,Unit[] units)
    {
        //just 1 unit has been selected
        if (units.Length == 1)
        {
            units[0].GatherResource(resource, UnitMover.GetUnitDestinationAroundResource(resource.transform.position));
        }
        //multiple units have been selected
        else
        {
            Vector3[] destinations = UnitMover.GetUnitGroupDestinationsAroundResource(resource.transform.position, units.Length);
            //get a number of evenly spaced positions to send the different units to, so they dont all clump up
            for(int x = 0; x < units.Length; x++)
            {
                units[x].GatherResource(resource, destinations[x]);
            }

        }
    }
    //call when we command unit to attack an enemy
    void UnitsAttackEnemy(Unit target, Unit[] units)
    {
        for(int x = 0; x < units.Length; x++)
        {
            units[x].AttackUnit(target);
        }
    }


    //creates a new selection marker visual at the given position
    void CreateSelectionMarker (Vector3 pos, bool large)
    {

        GameObject marker = Instantiate(selectionMarkerPrefab, new Vector3 (pos.x, 0.01f,pos.z),Quaternion.identity);

        if (large)
        {
            marker.transform.localScale = Vector3.one * 3;
        }
    }

}

