namespace Teslow_srv.Domain.Dto.Game;

public class AddScoreGameDto
{
    public int Team1 { get; set; }  // score de la première équipe
    public int Team2 { get; set; }  // score de la deuxième équipe
    public int Duration { get; set; }  // durée du match en secondes
}