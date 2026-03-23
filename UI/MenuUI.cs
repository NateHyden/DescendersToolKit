using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.UI
{
    public static class MenuUI
    {
        private static GameObject menuCanvas;
        private static bool menuVisible;
        private static CursorLockMode prevLock;
        private static bool prevVis;

        public static void ToggleMenu()
        {
            if (menuCanvas == null) menuCanvas = MenuWindow.CreateMenu();
            menuVisible = !menuVisible;
            menuCanvas.SetActive(menuVisible);
            if (menuVisible)
            {
                prevLock = Cursor.lockState; prevVis = Cursor.visible;
                Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
                if (MenuWindow.RootCanvasGroup != null) MenuWindow.RootCanvasGroup.alpha = 1f;
            }
            else RestoreCursor();
        }

        public static void RestoreCursor()
        {
            Cursor.lockState = prevLock; Cursor.visible = prevVis;
        }
    }
}
