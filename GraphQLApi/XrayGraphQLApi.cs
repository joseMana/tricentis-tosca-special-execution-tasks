using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Tricentis.Tosca.Integration.JiraXray.GraphQLApiMethods;
using Tricentis.Tosca.Integration.JiraXray;

namespace Tricentis.Tosca.Integration.JiraXray.GraphQLApi
{
    internal class XrayGraphQLApi : IXrayAPI
    {
        public GraphQLHttpClient GraphQLClient
        {
            get
            {
                var _graphQLClient = new GraphQLHttpClient(ConfigurationManager.AppSettings["xrayCloudEndPoint"], new NewtonsoftJsonSerializer());
                _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", BearerToken);

                return _graphQLClient;
            }
            set 
            {
            }
        }
        private string BearerToken
        {
            get
            {
                HttpClient client = new HttpClient();
                var values = new Dictionary<string, string>
                {
                    { "client_id", ConfigurationManager.AppSettings["clientId"] },
                    { "client_secret", ConfigurationManager.AppSettings["clientSecret"] }
                };
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync(ConfigurationManager.AppSettings["xrayCloudAuthenticationEndPoint"], content);
                var responseString = response.Result.Content.ReadAsStringAsync().Result.Replace("\"", "");

                return responseString;
            }
        }
        public XrayAPIMethods Methods { get; set; }
        public XrayGraphQLApi()
        {
            Methods = new XrayGraphQLApiMethods(GraphQLClient);
        }
        public XrayGraphQLApi(GraphQLHttpClient graphQLClient)
        {
            Methods = new XrayGraphQLApiMethods(graphQLClient);
        }
    }
}
