using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Crafting_Recipe", order = 1)]
public class Script_CraftRecipe : ScriptableObject
{
    public string recipeName;
    public int gridSizeX;
    public int gridSizeY;
    public string recipeRawKey;
    public string recipeKey;
    public Script_InvItem weaponTemplate;
}
