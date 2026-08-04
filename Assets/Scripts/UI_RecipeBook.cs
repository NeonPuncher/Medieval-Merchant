using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class UI_RecipeBook : MonoBehaviour
{
    public int pageNum;
    public GameObject recipeImg;
    public List<Texture> availableRecipes;

    private void Start()
    {
        pageNum = 0;
    }

    public void UpdatePage()
    {
        recipeImg.GetComponent<Renderer>().materials[0].SetTexture("_BaseMap", availableRecipes[pageNum]);
    }

    public void PageUp()
    {
        if(pageNum >= 0 && pageNum < availableRecipes.Count - 1)
        {
            pageNum++;
            UpdatePage();
        }

    }

    public void PageDown()
    {
        if(pageNum > 0)
        {
            pageNum--;
            UpdatePage();
        }
    }
}


