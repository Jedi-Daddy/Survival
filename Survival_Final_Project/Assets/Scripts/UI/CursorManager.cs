using UnityEngine;

public static class CursorManager
{
    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void SetCursorState(bool isVisible)
    {
        if (isVisible)
        {
            UnlockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }
}
