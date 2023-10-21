using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Trailfin.Application.Models.Users;
using Trailfin.Tests.Helpers;
using Trailfin.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Trailfin.Tests.ControllersTests
{
    public class AuthControllerTests : IntegrationTest
    {
        private HttpClient _api;
        public AuthControllerTests()  {
             _api = _factory.CreateClient();
        }
        [Fact]
        public async Task UpdateName_Error_IfWrongUserId()
        {
            var impossibleId = 1651684186684181864;
            UpdateNameRequestDto body = new UpdateNameRequestDto
            {
                Pseudo = "pseudotest",
                LastName = "lastnameTest",
                FirstName = "firstNameTest",
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "Application/json");
            _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var sut = await _api.PutAsync($"api/auth/name/{impossibleId}", content);
            sut.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateName_OK()
        {
            var impossibleId = 1;
            UpdateNameRequestDto body = new UpdateNameRequestDto
            {
                Pseudo = "pseudotest",
                LastName = "lastnameTest",
                FirstName = "firstNameTest",
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "Application/json");
            _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var sut = await _api.PutAsync($"api/auth/name/{impossibleId}", content);
            sut.IsSuccessStatusCode.Should().BeTrue();
            var response = await sut.Content.ReadAsStringAsync();
            var envelope = JsonConvert.DeserializeObject<Envelope<string>>(response);
            CleanDatabase("UPDATE user SET pseudo = null, lastname = NULL, firstname = null WHERE id = 1;");
        }

    }
}
