using FakeIdentityBot.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FakeIdentityBot.IdentityClient
{
    public class FakeIdentityClient
    {

        public async Task<FakeIdentity> GetFakeIdentity()
        {
            FakeIdentity fakeIdentity = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3979/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("https://uinames.com/api/");
                if (response.IsSuccessStatusCode)
                {
                    fakeIdentity = await response.Content.ReadAsAsync<FakeIdentity>();
                }
            }

            return fakeIdentity;
        }
    }
}