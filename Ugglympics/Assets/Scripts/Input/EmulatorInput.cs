using UnityEngine;

class EmulatorInput : InputAdapter
{
    public override bool GetInput()
    {
        return Input.GetMouseButton(0);
    }

    public override bool GetInputDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override bool GetInputUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public override Vector3 GetInputPosition()
    {
        return Input.mousePosition;
    }
}
