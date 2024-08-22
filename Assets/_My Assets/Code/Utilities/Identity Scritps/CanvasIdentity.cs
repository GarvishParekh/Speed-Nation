using UnityEngine;


public class CanvasIdentity : MonoBehaviour
{
    [SerializeField] private CanvasNames myCanvas;

    public CanvasNames GetCanvasName()
    {
        return myCanvas;
    }
}

public enum CanvasNames
{
    MAIN_MENU,
    GARAGE,
    GAMEPLAY,
    PAUSE,
    GAMEOVER,
    OPTIONS,
    SHOP
}
