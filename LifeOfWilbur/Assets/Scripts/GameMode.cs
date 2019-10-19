public enum GameMode
{
    /// <summary>
    /// No game is in progress, i.e. the player is in a menu other than the pause menu.
    /// </summary>
    NotInGame,

    /// <summary>
    /// The game is in story mode. In story mode, NPC conversation levels occur, but the game timer is not
    /// shown. This encourages the player to take in the deep mapping without having to worry about their
    /// score.
    /// </summary>
    Story,

    /// <summary>
    /// The game is in speed-run mode. NPC conversation levels do not occur. The player's goal is to have
    /// the fastest time. This mode allows for the game to be replayed, and players can compete with online
    /// hiscores.
    /// 
    /// Story mode must be completed before speed run mode is unlocked.
    /// </summary>
    SpeedRun,
}

static class GameModeExtensions
{
    public static bool IsInGame(this GameMode gameMode)
    {
        return gameMode != GameMode.NotInGame;
    }
}