using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Models;

[Table("LoginRequest")]
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}