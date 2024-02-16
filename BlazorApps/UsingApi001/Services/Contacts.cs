using Newtonsoft.Json;
using System.Text;
using UsingApi001.Model;

namespace UsingApi001.Services
{
    public class Contacts
    {
        private readonly IConfiguration _configuration;

        public Contacts(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            var _endpoint = _configuration.GetValue<string>("URLs:UrlContacts");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(_endpoint);
                var contacts = JsonConvert.DeserializeObject<List<Contact>>(response);
                return contacts;
            }
        }
        public async Task<Contact> GetContactById(string Id)
        {
            var _endpoint = _configuration.GetValue<string>("URLs:UrlContacts");

            _endpoint = $"{_endpoint}/{Id}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(_endpoint);
                var contact = JsonConvert.DeserializeObject<Contact>(response);
                return contact;
            }
        }

        public async Task<Contact> SaveNewContact(Contact newContact)
        {
            var _endpoint = _configuration.GetValue<string>("URLs:UrlContacts");

            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(newContact);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(_endpoint, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var contact = JsonConvert.DeserializeObject<Contact>(responseContent);
                    return contact;
                }
                else
                {
                    // Handle the error if the request is not successful
                    // You might want to throw an exception or return a specific result
                    // based on the error scenario.
                    throw new HttpRequestException($"Failed to save new contact. Status Code: {response.StatusCode}");
                }
            }
        }

    }
}
