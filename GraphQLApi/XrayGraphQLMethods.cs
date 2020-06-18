using System;
using GraphQL;
using GraphQL.Client.Http;
using System.Threading.Tasks;
using Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Classes;
using Tricentis.Tosca.Integration.JiraXray.GraphQLApi.Queries;
//TODO 1 : Might create a helper class for the 'Private Methods' region
namespace Tricentis.Tosca.Integration.JiraXray.GraphQLApiMethods
{
    internal class XrayGraphQLApiMethods : XrayAPIMethods 
    {
        private GraphQLHttpClient GraphQLClient { get; set; }
        private GraphQLRequest GraphQLRequest { get; set; }
        public IGraphQLResponse GraphQLResponse { get; set; }
        public XrayGraphQLApiMethods() { }
        public XrayGraphQLApiMethods(GraphQLHttpClient graphQLClient) 
        {
            GraphQLClient = graphQLClient;
        }
        public override Task<IGraphQLResponse> GetTestById(string testId)
        {
            return SendXrayGraphQLQuery<GetTestByIdResponse>(str => str.Replace("[VAR1]", testId));
        }
        public override Task<IGraphQLResponse> UpdateTestRunStatusById(string testRunId, TestStatus testStatus)
        {
            return SendXrayGraphQLQuery<UpdateTestRunStatusByIdResponse>(str => str.Replace("[VAR1]", testRunId).Replace("[VAR2]", testStatus.ToString()));
        }
        #region Private Methods
        private async Task<IGraphQLResponse> SendXrayGraphQLQuery<T>(Func<string, string> postStringFunction) where T : GraphQLApiResponse, new()
        {
            try
            {
                GraphQLRequest.Query = XrayGraphQLQueries.GetQueryByType(new T(), postStringFunction);
                GraphQLResponse = await GraphQLClient.SendQueryAsync<T>(GraphQLRequest);

                return GraphQLResponse;
            }
            catch (Exception)
            {
                return GraphQLResponse;
            }
        }
        #endregion
    }
    public class XrayGraphQLQueries
    {
        public const string GetTestByIdQuery = "{" +
                                                    "getTest(issueId: \"[VAR1]\") " +
                                                    "{" +
                                                         "issueId\n" +
                                                         "testType\n " +
                                                         "{" +
                                                             "name\n" +
                                                             "kind\n" +
                                                         "}" +
                                                         "steps " +
                                                         "{" +
                                                             "id\n" +
                                                             "data\n" +
                                                             "action\n" +
                                                             "result\n" +
                                                             "attachments " +
                                                             "{" +
                                                                 "id\n" +
                                                                 "filename\n" +
                                                             "}" +
                                                         "}" +
                                                    "}" +
                                               "}";

        public const string UpdateTestRunStatusQuery = "mutation " +
                                                       "{" +
                                                            "updateTestRunStatus(id: \"[VAR1]\", status: \"[VAR2]\")" +
                                                       "}";

        internal static string GetQueryByType(GraphQLApiResponse response, Func<string, string> postStringFunction)
        {
            string retVal = "";

            GetTestByIdResponse getTestByIdResponse = response as GetTestByIdResponse;
            if (getTestByIdResponse != null)
            {
                retVal = GetTestByIdQuery;
            }

            UpdateTestRunStatusByIdResponse updateTestRunResponse = response as UpdateTestRunStatusByIdResponse;
            if (updateTestRunResponse != null)
            {
                retVal = GetTestByIdQuery;
            }

            return postStringFunction(retVal);
        }
    }
}
