using Raylib_cs;

namespace GameEngine.src.input;
public class InputMap
{
    private Dictionary<string, KeyboardKey> keyBindings;

    internal InputMap()
    {
        keyBindings = new Dictionary<string, KeyboardKey>();

        // Default actions, can be modified later
        AssignKey("up", KeyboardKey.Up);
        AssignKey("down", KeyboardKey.Down);
        AssignKey("left", KeyboardKey.Left);
        AssignKey("right", KeyboardKey.Right);
    }

    // Method to assign a key to a specific action
    // Creates action if does not exist
    public void AssignKey(string action, KeyboardKey key)
    {
        if (!keyBindings.ContainsKey(action))
            keyBindings.Add(action, key);

        else keyBindings[action] = key;
    }

    // Modify an already assigned key for a specific action
    // Creates action if does not exist
    public void ModifyKey(string action, KeyboardKey newKey)
    {
        if (keyBindings.ContainsKey(action))
            keyBindings[action] = newKey;

        else AssignKey(action, newKey);
    }

    // Removes an already assigned key for a specific action
    // Does nothing if action does not exist
    public void RemoveKey(string action)
    {
        if (keyBindings.ContainsKey(action))
            keyBindings.Remove(action);

        else return;
    }

    // Method to get the key assigned to a specific action
    // Returns null if action does not exist
    public KeyboardKey GetKey(string action)
    {
        if (keyBindings.ContainsKey(action))
            return keyBindings[action];

        else return KeyboardKey.Null;
    }

}

