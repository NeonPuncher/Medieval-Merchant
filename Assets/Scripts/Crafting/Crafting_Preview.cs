using System.Collections.Generic;

using NUnit.Framework;

using UnityEngine;
using UnityEngine.Assertions.Must;

using static UnityEditor.Progress;

public class Crafting_Preview : MonoBehaviour
{
    public enum CraftingPreviewState
    {
        POSITIVE,

        NEGATIVE,
    }

    [SerializeField] private Material positiveMaterial;
    [SerializeField] private Material negativeMaterial;

    public CraftingPreviewState State { get;  private set; } = CraftingPreviewState.NEGATIVE;
    public Script_InvItem Data { get; private set; }
    public Crafting_Model CraftingModel;
    public GameObject model;

    private List<Renderer> renderers = new();
    private List<Collider> colliders = new();

    public void Setup(Script_InvItem item)
    {
        Data = item;
        CraftingModel = Instantiate(item.gridModel,transform.position, Quaternion.identity, transform);
        model = Instantiate(item.Model, transform.position, Quaternion.identity, transform);
        model.transform.Rotate(90, 0, 0);
        renderers.AddRange(CraftingModel.GetComponentsInChildren<Renderer>());
        colliders.AddRange(CraftingModel.GetComponentsInChildren<Collider>());

        foreach(var col in colliders)
        {
            col.enabled = false;
        }
        SetPreviewMaterial(State);

    }

    public void ChangeState(CraftingPreviewState newState)
    {
        if (newState == State) return;
        State = newState;
        SetPreviewMaterial(State);
    }

    public void Rotate(int rotationStep)
    {
        CraftingModel.Rotate(rotationStep);
        model.transform.Rotate(0, 0, -90);
    }

    private void SetPreviewMaterial(CraftingPreviewState newState)
    {
        Material previewMat = newState == CraftingPreviewState.POSITIVE ? positiveMaterial : negativeMaterial;
        foreach (var rend in renderers)
        {
            Material[] mats = new Material[rend.sharedMaterials.Length];
            for(int i = 0; i < mats.Length; i++)
            {
                mats[i] = previewMat;
            }
            rend.materials = mats;
        }
    }
}
