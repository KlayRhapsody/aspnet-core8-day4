namespace WebApi.Models.Dtos;

public class InstructorsResponse
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Discriminator { get; set; } = null!;
}