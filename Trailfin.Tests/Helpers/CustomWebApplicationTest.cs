using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Trailfin.Application.Models;
using Trailfin.Application.Models.Helpers;
using Trailfin.Application.Models.Users;
using Trailfin.Domain.Entitites;
using Trailfin.Web.Helpers;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Trailfin.Tests.Helpers
{
    public class CustomWebApplicationTest<IProgram> : WebApplicationFactory<IProgram> where IProgram : class
    {
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            var config = new ConfigurationBuilder();
            return config
                .AddUserSecrets<TestConfiguration>()
                .Build();
        }

       
       
    }
}
