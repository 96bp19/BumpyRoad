
using UnityEngine;

public static class InputHandler 
{
   
    public static bool IsScreenTapped()
    {
        return Input.GetMouseButton(0);
    }
}
