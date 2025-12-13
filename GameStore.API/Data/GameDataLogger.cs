namespace GameStore.API.Data
{
    public class GameDataLogger(GameStoreData store, ILogger<GameDataLogger> logger)
    {
        public void PrintGames()
        {
            var games = store.GetAllGames();
            if (games == null || !games.Any())
            {
                logger.LogInformation("No games found");
                return;
            }

            foreach (var game in games)
            {
                logger.LogInformation("Game Id: {GameId}, Name: {GameName}", game.Id, game.Name);
            }
        }
    }
}