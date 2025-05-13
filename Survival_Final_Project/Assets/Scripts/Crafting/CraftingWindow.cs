using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingWindow : MonoBehaviour
{
    public CraftingRecipeUI[] recipeUIs;

    public static CraftingWindow instance;
    private bool isCraftingWindowOpen = false;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventory);
        CursorManager.SetCursorState(true);  
    }

    void OnDisable()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventory);
        RestorePlayerControl();  
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            ToggleCraftingWindow();
        }
    }

    void OnOpenInventory()
    {
        CloseCraftingWindow();
    }

    private void OpenCraftingWindow()
    {
        gameObject.SetActive(true);
        CursorManager.SetCursorState(true);
        LockPlayerControl();
        isCraftingWindowOpen = true;
    }

    private void CloseCraftingWindow()
    {
        gameObject.SetActive(false);
        CursorManager.SetCursorState(false);
        RestorePlayerControl();
        isCraftingWindowOpen = false;
    }

    public void ToggleCraftingWindow()
    {
        if (isCraftingWindowOpen)
            CloseCraftingWindow();
        else
            OpenCraftingWindow();
    }

    
    private void LockPlayerControl()
    {
        CursorManager.UnlockCursor();
        Time.timeScale = 0f;  
        PlayerController.instance.canLook = false;
    }

    
    private void RestorePlayerControl()
    {
        CursorManager.LockCursor();
        Time.timeScale = 1f;  
        PlayerController.instance.canLook = true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        for (int i = 0; i < recipe.cost.Length; i++)
        {
            for (int x = 0; x < recipe.cost[i].quantity; x++)
            {
                Inventory.instance.RemoveItem(recipe.cost[i].item);
            }
        }

        Inventory.instance.AddItem(recipe.itemToCraft);

        for (int i = 0; i < recipeUIs.Length; i++)
        {
            recipeUIs[i].UpdateCanCraft();
        }
    }
}
