using System.Collections.Generic;
using Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Classes;

namespace Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Queries
{
    public class GetTestByIdResponse: GraphQLApiResponse
    {
        public GetTest getTest { get; set; }
        public Data data { get; set; }
        public TestType testType { get; set; }
        public Root root { get; set; }
        public class TestType
        {
            public string name { get; set; }
            public string kind { get; set; }
        }
        public class GetTest
        {
            public string issueId { get; set; }
            public TestType testType { get; set; }
            public List<object> steps { get; set; }
        }
        public class Data
        {
            public GetTest getTest { get; set; }
        }
        public class Root
        {
            public Data data { get; set; }
        }
    }
    
}
