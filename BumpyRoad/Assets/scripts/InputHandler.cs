
using UnityEngine;

public static class InputHandler 
{
    public delegate void OnInputReceived();
    public static OnInputReceived inputReceivedListeners;
   
    public static bool IsScreenTapped()
    {
        bool screenTapped = Input.GetMouseButton(0);
        if (screenTapped)
        {
            inputReceivedListeners?.Invoke();

        }
        return screenTapped; 

    }
}
