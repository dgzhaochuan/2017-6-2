

using UnityEngine;

public class WorldUI:MonoBehaviour
{
   static private WorldUI instance;
   static public WorldUI Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject game = GameObject.Find("WorldUI");
                instance = game.GetComponent<WorldUI>();
                if (instance == null)
                    instance = game.AddComponent<WorldUI>();
            }
            return instance;
        }
    }

    Transform hudParent;
    public Transform HUDParent
    {
        get
        {
            if (hudParent == null)
            {
                hudParent = transform.Find("HUD");
            }
            return hudParent;
        }
    }



}