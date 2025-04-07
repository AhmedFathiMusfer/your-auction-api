namespace your_auction_api.Models.Dto
{
  public class TokenDTO
  {
    public UserDTO? UserData { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
  }
}
