using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Trailfin.Tests.Helpers
{
    public class IntegrationTest
    {
        protected readonly CustomWebApplicationTest<Program> _factory;
        protected string _token;

        public IntegrationTest()
        {
            _factory = new CustomWebApplicationTest<Program>();
            _token = generateJwtToken();
        }

        protected void CleanDatabase(string query)
        {

            string connectionString = TestConfiguration.GetValue<string>("ConnectionStrings:DefaultConnection");//z "Server=localhost;port=3307;Database=trailfin;User Id=root;Password=;";/*TODO : secretisé*/
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private string generateJwtToken()
        {
            try
            {
                string emailTest = TestConfiguration.GetValue<string>("Test:Email");
                string secretKey = TestConfiguration.GetValue<string>("AppSettings:Secret");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("email", emailTest) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
