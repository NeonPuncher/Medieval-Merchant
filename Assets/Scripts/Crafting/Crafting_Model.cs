using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;

using UnityEngine;

public class Crafting_Model : MonoBehaviour
{

    [SerializeField] private Transform wrapper;

    public float Rotation => wrapper.transform.eulerAngles.y;
    private Crafting_ShapeUnit[] shapeUnits;

    private void Awake()
    {
        shapeUnits = GetComponentsInChildren<Crafting_ShapeUnit>();
    }

    public void Rotate(float rotationStep)
    {
        wrapper.Rotate(new(0, rotationStep, 0));
    }

    public List<Vector3> GetAllCraftingPositions()
    {
        return shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}
