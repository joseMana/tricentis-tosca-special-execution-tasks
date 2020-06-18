using Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Classes;

namespace Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Queries
{
    public class UpdateTestRunStatusByIdResponse : GraphQLApiResponse
    {
        public Data data { get; set; }
        public Root root { get; set; }
        public class Data
        {
            public string updateTestRunStatus { get; set; }
        }
        public class Root
        {
            public Data data { get; set; }
        }
    }
}
