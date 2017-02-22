public class Events
{
    public class InputEvents
    {
        public const string validSwipeCaptured = "validSwipeCaptured";
        public const string validSwipeSetCaptured = "validSwipeSetCaptured";
    }

    public class UIEvents
    {
        public const string hitButtonClick = "hitButtonClick";
        public const string resetButtonClick = "resetButtonClick";
        public const string cheatButtonClick = "cheatButtonClick";
    }

    public class PlayerDataEvents
    {
        public const string swipeMeterValueChanged = "swipeMeterValueChanged";
        public const string hitMeterValueChanged = "hitMeterValueChanged";
        public const string stunStateChanged = "stunStateChanged";
    }

    public class GameEvents
    {
        public const string swipePhaseComplete = "swipePhaseComplete";
        public const string playerConnected = "playerConnected";
    }

    public class CheatEvents
    {
        public const string incrementCheatValue = "incrementCheatValue";
        public const string decrementCheatValue = "decrementCheatValue";
        public const string refreshCheats = "refreshCheatValue";
    }
}
