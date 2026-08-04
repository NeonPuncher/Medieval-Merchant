using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haggle_Item : MonoBehaviour
{
    public RectTransform imgRectTransform;
    private Haggle_ItemManager itemManager;
    public Sprite attachSprite;
    private Vector3 mousePosition;
    public Script_InvItem itemInfo;
    public string itemType;
    public Material itemMaterial;

    void Start()
    {
        imgRectTransform = GameObject.Find("CraftingBounds").GetComponent<RectTransform>();
        itemManager = FindAnyObjectByType<Haggle_ItemManager>();
        //Set Info from Scriptable object and Generate attachpoints

        GameObject itemModel = Instantiate(itemInfo.Model, this.transform);
        itemModel.GetComponent<MeshRenderer>().materials[0] = itemMaterial;
        itemModel.transform.localPosition = new Vector3(0, 0, 0);
        itemModel.transform.localScale = new Vector3(30, 30, 30);
        itemModel.transform.rotation = Quaternion.Euler(270, 0, 180);

        itemManager.itemName = itemInfo.name;
        itemManager.itemValue = itemInfo.value;
        itemManager.totalValue = itemManager.totalValue + itemInfo.value;
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDrag()
    {

        //Get mouse position
        Vector3 localMousePosition = imgRectTransform.InverseTransformPoint(Input.mousePosition);

        // Mouse must be inside bounds of crafting table
        if (imgRectTransform.rect.Contains(localMousePosition))
        {
            // Cache current Y position (so it doesn't move up/down)
            float fixedY = 1f;

            // Get mouse position in world space
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z)
            );

            // Apply only X and Z movement, keep Y stable
            transform.position = new Vector3(mouseWorldPos.x, fixedY, mouseWorldPos.z);

            // Rotate with scroll wheel
            transform.Rotate(0, 0, Input.mouseScrollDelta.y * 90);
        }
        else
        {
            Debug.Log("Mouse outside bounds");
        }
    }
}
