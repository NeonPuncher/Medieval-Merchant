using System.Collections.Generic;

using UnityEngine;

public class Crafting_Item : MonoBehaviour
{
    //Extra functionality for the Crafting item after it was placed

    private Crafting_Model model;
    private GameObject itemModel;
    public Script_InvItem data;
    public Script_HaggleAbility abilityData;
    public string itemName;
    private List<Renderer> renderers = new();


    public void Setup(Script_InvItem data, float rotation, List<int> matType)
    {
        this.data = data;
        model = Instantiate(data.gridModel, transform.position, Quaternion.identity, transform);
        itemModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        itemModel.gameObject.transform.Rotate(90f,0,0,Space.Self);

        renderers.AddRange(model.GetComponentsInChildren<Renderer>());
        for (int r = 0; r < renderers.Count; r++)
        {
            Material[] mats = new Material[renderers[r].sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = data.Materials[matType[r]];
            }
            renderers[r].materials = mats;
        }

        model.Rotate(rotation);
        itemModel.gameObject.transform.Rotate(0,0,-rotation,Space.Self); 
    }

    public void SetUpAbility (Script_HaggleAbility abilityData, float rotation, List<int> matType)
    {
        this.abilityData = abilityData;
        model = Instantiate(abilityData.gridModel, transform.position, Quaternion.identity, transform);
        //itemModel = Instantiate(abilityData.Model, transform.position, Quaternion.identity, transform);
        //itemModel.gameObject.transform.Rotate(90f, 0, 0, Space.Self);

        renderers.AddRange(model.GetComponentsInChildren<Renderer>());
        for (int r = 0; r < renderers.Count; r++)
        {
            Material[] mats = new Material[renderers[r].sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = abilityData.Materials[matType[r]];
            }
            renderers[r].materials = mats;
        }

        model.Rotate(rotation);
        //itemModel.gameObject.transform.Rotate(0, 0, -rotation, Space.Self);
    }
}
