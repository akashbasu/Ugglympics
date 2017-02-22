using UnityEngine;

public abstract class InputAdapter
{
    private static InputAdapter instance;

    public static InputAdapter Get()
    {
        if(instance == null)
        {
            switch(Application.platform)
            {
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    {
                        instance = new EmulatorInput();
                    }
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    {
                        instance = new MobileInput();
                    }
                    break;
                default:
                    break;

            }
        }

        return instance;
    }

    public abstract bool GetInputDown();
    public abstract bool GetInput();
    public abstract bool GetInputUp();
    public abstract Vector3 GetInputPosition();
}
