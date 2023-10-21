using MySqlConnector;

namespace Trailfin.Tests.Helpers
{
    public class TestConfiguration
    {
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            var config = new ConfigurationBuilder();
            return config
                .AddUserSecrets<TestConfiguration>()
                .Build();
        }

        protected void CleanDatabase(string query)
        {

            string connectionString = TestConfiguration.GetValue<string>("ConnectionStrings:DefaultConnection");//z "Server=localhost;port=3307;Database=trailfin;User Id=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static T GetValue<T>(string name)
        {

            var iConfig = GetIConfigurationRoot();
            if (!name.Contains(":"))
            {
                return iConfig
                    .GetValue<T>(name);
            }

            var value = iConfig.GetSection(name.Split(":")[0])
                .GetValue<T>(name.Split(":")[1]);

            if (value == null)
                throw new Exception($"Clé introuvable dans les secrets de tests: {name}");
            return value;
        }
    }
}
