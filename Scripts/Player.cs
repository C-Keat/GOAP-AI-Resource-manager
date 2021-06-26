using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    public bool isMe;

    [Header("Units")]
    public List<Unit> units = new List<Unit>();

    [Header("Resources")]
    public int food;

    [Header("Components")]
    public GameObject unitPrefab;
    public Transform unitSpawnPos;

    //events 
    [System.Serializable]
    public class UnitCreatedEvent : UnityEvent<Unit> { }
    public UnitCreatedEvent onUnitCreated;

    public readonly int unitCost = 50;

    public static Player me;

    private void Awake()
    {
        if (isMe)
        {
            me = this;
        }
        
    }
    private void Start()
    {
        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateFoodText(food);

        CameraController.instance.FocusOnPosition(unitSpawnPos.position);

        food += unitCost;
        CreateNewUnit();
    }


    // called when a unit gathers a certain resource
    public void GainResource (ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
            {
                food += amount;
                GameUI.instance.UpdateFoodText(food);
                break;
            }

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CreateNewUnit();
        }
    }

    public void CreateNewUnit()
    {
        if (food - unitCost < 0)
        {
            return;
        }

        GameObject UnitObj = Instantiate(unitPrefab, unitSpawnPos.position, Quaternion.identity, transform);
        Unit unit = UnitObj.GetComponent<Unit>();

        units.Add(unit);
        unit.player = this;
        food -= unitCost;

        if (onUnitCreated != null)
        {
            onUnitCreated.Invoke(unit);
        }

        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateFoodText(food);

    }

    //is this my unit or an enemy unit
    public bool IsMyUnit (Unit unit)
    {
        return units.Contains(unit);
    }
}
