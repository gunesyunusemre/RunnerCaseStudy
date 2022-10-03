namespace Player
{
    public static class PlayerHelper
    {
        private static PlayerContainer _playerContainer;
        private static int _playerID;

        public static void SetPlayerContainer(this PlayerController playerController, PlayerContainer playerContainer, int instanceID)
        {
            _playerContainer = playerContainer;
            _playerID = instanceID;
        }

        public static bool TryGetPlayerContainer(int id, out PlayerContainer container)
        {
            container = default;
            var check = id == _playerID;
            if (check)
                container = _playerContainer;

            return check;
        }
    }
}