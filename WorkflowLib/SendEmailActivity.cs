using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OptimaJet.Workflow.Core;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;

namespace WorkflowLib;

public class SendEmailActivity : ActivityBase
{
    public SendEmailActivity() : base()
    {
        Type = "SendEmailActivity";
        Title = "Send Email Activity";
        Description = "The action is mine";

        // the file name with your form template, without extension
        Template = "sendEmailTemplate";
        // the file name with your svg template, without extension
        SVGTemplate = "SendEmailSVGTemplate";

        Parameters.Add(new CodeActionParameterDefinition()
        {
            Name = "MyField",
            Type = ParameterType.TextArea
        }
        );
    }

    public override async Task ExecutionAsync(WorkflowRuntime runtime,
        ProcessInstance processInstance, Dictionary<string, string> parameters,
        CancellationToken token)
    {
        Console.WriteLine("MyCustomActivity:");
        foreach (var item in parameters)
            Console.WriteLine($"{item.Key} - {item.Value}");
        Console.WriteLine("--------------");
    }


    public static async Task SendEmailAction(
    WorkflowRuntime runtime,
    ProcessInstance processInstance,
    Dictionary<string, string> parameters,
    CancellationToken token)
    {
        string to = parameters.ContainsKey("To") ? parameters["To"] : "";
        string subject = parameters.ContainsKey("Subject") ? parameters["Subject"] : "Default Subject";
        string body = parameters.ContainsKey("Body") ? parameters["Body"] : "No content";

        // You can send an email here — for demo purposes we log to console
        Console.WriteLine($"Sending email to {to} with subject '{subject}' and body: {body}");

        await Task.CompletedTask;
    }


    public override async Task PreExecutionAsync(WorkflowRuntime runtime,
        ProcessInstance processInstance, Dictionary<string, string> parameters,
        CancellationToken token)
    {

    }
}