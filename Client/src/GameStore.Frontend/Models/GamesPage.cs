namespace GameStore.Frontend.Models;

using System.Text.Json.Serialization;

public record class GamesPage(int TotalPages, [property: JsonPropertyName("Games")] IEnumerable<GameSummary> Data);