using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string GetGameEndpointName = "GetGameById";

List<GameDto> games = new()
{
    new GameDto(1, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateOnly(2015, 5, 19)),
    new GameDto(2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new GameDto(3, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18)),
    new GameDto(4, "Among Us", "Party", 4.99m, new DateOnly(2018, 6, 15)),
    new GameDto(5, "Hades", "Roguelike", 24.99m, new DateOnly(2020, 9, 17))
};

app.MapGet("games", () => games);

app.MapGet("games/{id}", (int id) => {
    GameDto? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});
app.MapPost("games", (CreateGameDto newGame) => 
{
    GameDto gameDto = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(gameDto);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
});

app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) => {
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1) {
        return Results.NotFound();
    }
    
    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre, 
        updateGame.Price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});

app.MapDelete("games/{id}", (int id) => {
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent(); 
});

app.Run();
