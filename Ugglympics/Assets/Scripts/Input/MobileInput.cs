using UnityEngine;

class MobileInput : InputAdapter
{
    public override bool GetInput()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
            return true;

        return false;
    }

    public override bool GetInputDown()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            return true;

        return false;
    }

    public override bool GetInputUp()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
            return true;

        return false;
    }

    public override Vector3 GetInputPosition()
    {
        return Input.GetTouch(0).position;
    }
}
