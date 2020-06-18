using System;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Execution.Context;
using Tricentis.Automation.Execution.Results;
using Tricentis.Automation.Engines.Monitoring;
using Tricentis.Automation.AutomationInstructions.TestActions;
using Tricentis.Tosca.Integration.JiraXray.Helpers;
using System.Configuration;
namespace Tricentis.Tosca.Integration.JiraXray
{
    public class ToscaTestsSubscriber : MonitoringTaskExecutor
    {
        private readonly bool _isJiraXrayIntegrationEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["enableJiraXrayIntegration"]);
        private readonly bool _isDebuggerEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["enableDebugger"]);
        private IXrayAPI XrayAPI { get; set; }
        public ToscaTestsSubscriber(Validator validator) : base(validator)
        {
            XrayAPI = (_isJiraXrayIntegrationEnabled) ? new XrayGraphQLApi() : null;
        }
        public override void PreExecution()
        {
            try
            {
                ToscaTaskExecutionLogger.WriteToLogFile(
                    "========================" + Constants.NewLine +
                    "BEFORE TestCase: " + Constants.NewLine +
                    "========================" + Constants.NewLine +
                    "IsRetrying: " + RunContext.IsExecutionEntryRetrying + Constants.NewLine +
                    "TestCase: Name: " + RunContext.GetAdditionalExecutionInfo(Constants.TestCaseName) + Constants.NewLine +
                    "Nodepath: " + RunContext.GetAdditionalExecutionInfo(Constants.TestCaseNodepath) + Constants.NewLine +
                    "ExecEntry: Name: " + RunContext.GetAdditionalExecutionInfo(Constants.ExecutionEntryName) + Constants.NewLine +
                    "Nodepath: " + RunContext.GetAdditionalExecutionInfo(Constants.ExecutionEntryNodepath));
            }
            catch (Exception)
            {
            }
        }
        public override void PostExecution(Exception result)
        {
            try
            {
                ToscaTaskExecutionLogger.WriteToLogFile(
                    "========================" + Constants.NewLine +
                    "AFTER TestCase: " + Constants.NewLine +
                    "========================" + Constants.NewLine +
                    "IsRetrying: " + RunContext.IsExecutionEntryRetrying + Constants.NewLine +
                    "Exception occurred: " + result + Constants.NewLine +
                    "TestCase: Name: " + RunContext.GetAdditionalExecutionInfo(Constants.TestCaseName) + Constants.NewLine +
                    "Nodepath:" + RunContext.GetAdditionalExecutionInfo(Constants.TestCaseNodepath) + Constants.NewLine +
                    "ExecEntry: Name: " + RunContext.GetAdditionalExecutionInfo(Constants.ExecutionEntryName) + Constants.NewLine +
                    "Nodepath:" + RunContext.GetAdditionalExecutionInfo(Constants.ExecutionEntryNodepath));
            }
            catch (Exception)
            {
            }
        }
        public override void PostExecution(ExecutionResult result)
        {
            try
            {
                ToscaTaskExecutionLogger.WriteToLogFile(
                    "========================" + Constants.NewLine +
                    "AFTER TestCase:" + Constants.NewLine +
                    "========================" + Constants.NewLine +
                    "IsRetrying: " + RunContext.IsExecutionEntryRetrying + Constants.NewLine +
                    "TestCase passed: " + result.IsPositive() + Constants.NewLine +
                    "TestCase: Name: " + RunContext.GetAdditionalExecutionInfo(Constants.TestCaseName) + Constants.NewLine +
                    "Nodepath: " + RunContext.GetAdditionalExecutionInfo(Constants.TestCaseNodepath) + Constants.NewLine +
                    "ExecEntry: Name: " + RunContext.GetAdditionalExecutionInfo(Constants.ExecutionEntryName) + Constants.NewLine +
                    "Nodepath: " + RunContext.GetAdditionalExecutionInfo(Constants.ExecutionEntryNodepath));

            }
            catch (Exception)
            {
            }
        }
        public override void PostExecution(ITestAction testAction, ExecutionResult result)
        {
            #region 'MessageBox' for Debugging Purposes
            //MessageBox.Show("TestStep : " + testAction.Name.Value + "\n" + "TestStep Result : " + result.ResultState);
            #endregion

            ToscaTaskExecutionLogger.WriteToLogFile(
                    "\tTestStep Name : " + testAction.Name.Value + Constants.NewLine +
                    "\tTestStep Result State : " + result.ResultState + Constants.NewLine);

            var response = XrayAPI?.Methods.GetTestById("10006");
        }
    }
    public static class Constants
    {
        public static readonly string NewLine = Environment.NewLine;

        public const string ExecutionListNodepath = "executionlist.nodepath";
        public const string ExecutionListUniqueId = "executionlist.uniqueid";
        public const string ExecutionListName = "executionlist.name";
        public const string ExecutionListRevision = "executionlist.revision";
        public const string ExecutionEntryNodepath = "executionentry.nodepath";
        public const string ExecutionEntryUniqueId = "executionentry.uniqueid";
        public const string ExecutionEntryName = "executionentry.name";
        public const string ExecutionEntryRevision = "executionentry.revision";
        public const string TestCaseNodepath = "testcase.nodepath";
        public const string TestCaseUniqueId = "testcase.uniqueid";
        public const string TestCaseName = "testcase.name";
        public const string TestCaseRevision = "testcase.revision";
    }
    #region Serialize Object for Debugging Purposes
    //TemporaryTestClass t = new TemporaryTestClass();
    //t.TemporaryTestProperty = testAction;
    //[Serializable]
    //public class TemporaryTestClass
    //{
    //    public object TemporaryTestProperty { get; set; }
    //}
    ///// <summary>
    ///// Serializes an object.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="serializableObject"></param>
    ///// <param name="fileName"></param>
    //public void SerializeObject<T>(T serializableObject, string fileName)
    //{
    //    if (serializableObject == null) { return; }

    //    try
    //    {
    //        XmlDocument xmlDocument = new XmlDocument();
    //        XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
    //        using (MemoryStream stream = new MemoryStream())
    //        {
    //            serializer.Serialize(stream, serializableObject);
    //            stream.Position = 0;
    //            xmlDocument.Load(stream);
    //            xmlDocument.Save(fileName);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //Log exception here
    //    }
    //}


    ///// <summary>
    ///// Deserializes an xml file into an object list
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="fileName"></param>
    ///// <returns></returns>
    //public T DeSerializeObject<T>(string fileName)
    //{
    //    if (string.IsNullOrEmpty(fileName)) { return default(T); }

    //    T objectOut = default(T);

    //    try
    //    {
    //        XmlDocument xmlDocument = new XmlDocument();
    //        xmlDocument.Load(fileName);
    //        string xmlString = xmlDocument.OuterXml;

    //        using (StringReader read = new StringReader(xmlString))
    //        {
    //            Type outType = typeof(T);

    //            XmlSerializer serializer = new XmlSerializer(outType);
    //            using (XmlReader reader = new XmlTextReader(read))
    //            {
    //                objectOut = (T)serializer.Deserialize(reader);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //Log exception here
    //    }

    //    return objectOut;
    //}
    #endregion
}
