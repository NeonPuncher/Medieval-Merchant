using System.Collections.Generic;

using NUnit.Framework;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/CraftData", order = 1)]

public class Crafting_Data : ScriptableObject
{
    [field: SerializeField] public Crafting_Model Model { get; private set; }

    [field: SerializeField] public List<int> itemMat { get; private set; }
    [field: SerializeField] public string name { get; private set; }

    [field: SerializeField] public int shape {  get; private set; }

    [field: SerializeField] public List<Material> Materials { get; private set; }
    [field: SerializeField] public string Description {  get; private set; }
    [field: SerializeField] public string Cost { get; private set; }
}
