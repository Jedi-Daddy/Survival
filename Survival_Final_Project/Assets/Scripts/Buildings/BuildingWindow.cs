using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingWindow : MonoBehaviour
{
    private bool isBuildingWindowOpen = false;

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
        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ToggleBuildingWindow();
        }
    }

    void OnOpenInventory()
    {
        CloseBuildingWindow();
    }

    private void OpenBuildingWindow()
    {
        gameObject.SetActive(true);
        CursorManager.SetCursorState(true);
        LockPlayerControl();
        isBuildingWindowOpen = true;
    }

    private void CloseBuildingWindow()
    {
        gameObject.SetActive(false);
        CursorManager.SetCursorState(false);
        RestorePlayerControl();
        isBuildingWindowOpen = false;
    }

    public void ToggleBuildingWindow()
    {
        if (isBuildingWindowOpen)
            CloseBuildingWindow();
        else
            OpenBuildingWindow();
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
}
