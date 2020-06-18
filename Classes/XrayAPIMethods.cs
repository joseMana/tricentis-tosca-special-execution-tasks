using GraphQL;
using System.Threading.Tasks;
using Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Classes;

namespace Tricentis.Tosca.Integration.JiraXray
{
    public abstract class XrayAPIMethods
    {
        public abstract Task<IGraphQLResponse> GetTestById(string v);
        public abstract Task<IGraphQLResponse> UpdateTestRunStatusById(string testRunId, TestStatus testStatus);
    }
}