using MelonLoader;
using UnityEngine;

namespace DescendersModMenu.Mods
{
    public static class GiantEveryone
    {
        public static void SetScale(float scale)
        {
            try
            {
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].name == "Player_Networked")
                    {
                        Transform cyclist = all[i].transform.Find("Cyclist");
                        if ((object)cyclist != null) cyclist.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
            catch (System.Exception ex) { MelonLogger.Error("[GiantEveryone] SetScale: " + ex.Message); }
        }
    }
}
