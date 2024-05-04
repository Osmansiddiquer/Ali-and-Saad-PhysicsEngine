

using Raylib_cs;

namespace GameEngine.src.helper;

public struct GamePad
{
    // Dictionary of key bindings
    private static Dictionary<string, GamepadButton> controllerBindings = new Dictionary<string, GamepadButton>()
    {
        // Defualt values, can be modified later
        { "left", GamepadButton.LeftFaceLeft },
        { "right", GamepadButton.LeftFaceRight },
        { "up", GamepadButton.LeftFaceUp },
        { "down", GamepadButton.LeftFaceDown }
    };

    // Method to assign a button to a specific action
    // Creates action if does not exist
    public static void AssignButton(string action, GamepadButton button)
    {
        if (!controllerBindings.ContainsKey(action))
            controllerBindings.Add(action, button);
        else
            controllerBindings[action] = button;
    }

    // Modify an already assigned button for a specific action
    // Creates action if does not exist
    public static void ModifyButton(string action, GamepadButton newButton)
    {
        if (controllerBindings.ContainsKey(action))
            controllerBindings[action] = newButton;
        else
            AssignButton(action, newButton);
    }

    // Removes an already assigned button for a specific action
    // Does nothing if action does not exist
    public static void RemoveButton(string action)
    {
        if (controllerBindings.ContainsKey(action))
            controllerBindings.Remove(action);
        else
            return;
    }

    // Method to get the button assigned to a specific action
    // Returns null if action does not exist
    private static GamepadButton GetButton(string action)
    {
        if (controllerBindings.ContainsKey(action))
            return controllerBindings[action];
        else
            return GamepadButton.Unknown;
    }

    // Check for gamepad button input
    public static bool IsButtonPressed(string action, int player = 0) 
    { 
        return Raylib.IsGamepadButtonPressed(player, GetButton(action)); 
    }
    public static bool IsButtonDown(string action, int player = 0) 
    { 
        return Raylib.IsGamepadButtonDown(player, GetButton(action)); 
    }
    public static bool IsButtonUp(string action, int player = 0)
    {
        return Raylib.IsGamepadButtonUp(player, GetButton(action)); 
    }
    public static bool IsButtonReleased(string action, int player = 0) 
    { 
        return Raylib.IsGamepadButtonReleased(player, GetButton(action)); 
    }

   

}