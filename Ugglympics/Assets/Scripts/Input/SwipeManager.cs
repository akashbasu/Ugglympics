//#define LOG_POINTER_DATA
//#define LOG_SWIPE_DATA

using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    enum SwipeDirection
    {
        invaild,
        right,
        left
    }

    struct PointerData
    {
        public Vector3 position;
        public float time;
        public SwipeDirection direction;

        public new void ToString()
        {
#if(LOG_POINTER_DATA)
            Debug.LogFormat("Position : {0} Time : {1} Direction: {2}", position.x, time, (direction == SwipeDirection.right) ? "Right" : (direction == SwipeDirection.left) ? "Left" : "");
#endif
        }
    }

    struct ValidSwipe
    {
        public float time;
        public float speed;
        public SwipeDirection direction;

        public void SetSwipeData(float time, float speed, SwipeDirection direction)
        {
            this.time = time;
            this.speed = speed;
            this.direction = direction;
        }

        public new void ToString()
        {
#if(LOG_SWIPE_DATA)
            Debug.LogFormat("Speed : {0} Time : {1} Direction: {2}", speed, time, (direction == SwipeDirection.right) ? "Right" : (direction == SwipeDirection.left) ? "Left" : "");
#endif
        }
    }

    private float swipeDistance;
    private float swipeTime;
    private float swipeSpeed;
    private SwipeDirection swipeDirection;
    private bool touchPhaseActive;

    private List<PointerData> pointerDataPoints = new List<PointerData>();
    private List<ValidSwipe> validSwipes = new List<ValidSwipe>();
    
    void Update()
    {
        if (InputAdapter.Get().GetInputDown())
        {
            touchPhaseActive = true;

            CapturePointerData();
        }

        if (InputAdapter.Get().GetInput() && touchPhaseActive)
        {
            CapturePointerData();

            if (IsSameDirectionSwipe()) { }
            else OnSwipePhaseEnd();
        }

        if(InputAdapter.Get().GetInputUp() && touchPhaseActive)
        {
            touchPhaseActive = false;
            OnSwipePhaseEnd();
        }
    }
    
    void CapturePointerData()
    {
        PointerData pointerData = new PointerData();
        pointerData.position = InputAdapter.Get().GetInputPosition();
        pointerData.time = Time.time;
        pointerData.direction = FindDragDirection();

        if (pointerData.direction == SwipeDirection.invaild && pointerDataPoints.Count != 0)
            return;

        pointerData.ToString();
        pointerDataPoints.Add(pointerData);
    }

    SwipeDirection FindDragDirection()
    {
        if(pointerDataPoints.Count == 0)
            return SwipeDirection.invaild;

        float horizontalDistanceSinceLastDataPoint = Input.mousePosition.x - pointerDataPoints[pointerDataPoints.Count - 1].position.x;

        if (horizontalDistanceSinceLastDataPoint > 0)
            return SwipeDirection.right;
        else if (horizontalDistanceSinceLastDataPoint < 0)
            return SwipeDirection.left;
        else
            return SwipeDirection.invaild;
    }

    bool IsSameDirectionSwipe()
    {
        if (pointerDataPoints.Count <= 2)
            return true;

        int last = pointerDataPoints.Count - 1;
        int secondLast = pointerDataPoints.Count - 2;

        if (pointerDataPoints[secondLast].direction == pointerDataPoints[last].direction)
            return true;
        else
            return false;
    }

    void CalculateSwipeData()
    {
        int start = 1;
        int end = pointerDataPoints.Count - 2;

        swipeDistance = Mathf.Abs(pointerDataPoints[end].position.x - pointerDataPoints[start].position.x);
        swipeTime = Time.time;
        swipeDirection = pointerDataPoints[start].direction;
        swipeSpeed = swipeDistance / swipeTime;
    }

    bool IsLastSwipeValid()
    {
        if (swipeDistance < GameConstants.instance.minSwipeThreashold)
            return false;

        if (swipeTime < 0)
            return false;

        if ((validSwipes.Count > 0) && (swipeDirection == validSwipes[validSwipes.Count - 1].direction))
            return false;

        return true;
    }

    void CaptureSwipeData()
    {
        ValidSwipe validSwipe = new ValidSwipe();
        validSwipe.SetSwipeData(swipeTime, swipeSpeed, swipeDirection);
        validSwipe.ToString();
        validSwipes.Add(validSwipe);

        EventManager.SendMessage(Events.InputEvents.validSwipeCaptured, null);
    }

    void ResetPointerData()
    {
        swipeDistance = -1;
        swipeTime = -1;
        swipeSpeed = -1;
        swipeDirection = SwipeDirection.invaild;

        pointerDataPoints.Clear();
    }

    void OnSwipePhaseEnd()
    {
        if(pointerDataPoints.Count > 1)
            CalculateSwipeData();

        if (IsLastSwipeValid())
            CaptureSwipeData();

        ResetPointerData();
        CapturePointerData();
    }
}