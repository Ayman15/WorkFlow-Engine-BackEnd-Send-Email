using System.Xml.Linq;
using OptimaJet.Workflow.Core;
using OptimaJet.Workflow.Core.Builder;
using OptimaJet.Workflow.Core.Parser;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.DbPersistence;
using OptimaJet.Workflow.Plugins;

namespace WorkflowLib;

public static class WorkflowInit
{
    public const string ConnectionString = "Data Source=(local);Initial Catalog=WorkflowDB01;Integrated Security=True;TrustServerCertificate=True";

    private static readonly Lazy<WorkflowRuntime> LazyRuntime = new(InitWorkflowRuntime);
    private static readonly Lazy<MSSQLProvider> LazyProvider = new(InitMssqlProvider);

    public static WorkflowRuntime Runtime => LazyRuntime.Value;
    public static MSSQLProvider Provider => LazyProvider.Value;

    private static MSSQLProvider InitMssqlProvider()
    {
        return new MSSQLProvider(ConnectionString);
    }

    private static WorkflowRuntime InitWorkflowRuntime()
    {
        // TODO If you have a license key, you have to register it here
        //WorkflowRuntime.RegisterLicense("your license key text");

        var builder = new WorkflowBuilder<XElement>(
            Provider,
            new XmlWorkflowParser(),
            Provider
        ).WithDefaultCache();

        // we need BasicPlugin to send email
        var basicPlugin = new BasicPlugin
        {
            Setting_MailserverFrom = "ayman.imbabi@nozom-tech.com",
            Setting_Mailserver = "smtp.office365.com",
            Setting_MailserverSsl = true,
            Setting_MailserverPort = 587,
            Setting_MailserverLogin = "ayman.imbabi@nozom-tech.com",
            Setting_MailserverPassword = Secrets.MailPassword.ToString(),
        };
        var runtime = new WorkflowRuntime()
            .WithPlugin(basicPlugin)
            .WithBuilder(builder)
            .WithPersistenceProvider(Provider)
            .EnableCodeActions()
            .SwitchAutoUpdateSchemeBeforeGetAvailableCommandsOn()
            // add custom activity
            .WithCustomActivities(new List<ActivityBase> { new WeatherActivity() })
            // add custom rule provider
            .WithRuleProvider(new SimpleRuleProvider())
            .AsSingleServer();

        // events subscription
        runtime.OnProcessActivityChangedAsync += (sender, args, token) => Task.CompletedTask;
        runtime.OnProcessStatusChangedAsync += (sender, args, token) => Task.CompletedTask;

        runtime.Start();

        return runtime;
    }
}