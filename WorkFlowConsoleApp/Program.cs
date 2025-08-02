using WorkflowLib;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var runtime = WorkflowInit.Runtime;

        var schemeCode = "SimpleWF";
        var processId = Guid.NewGuid();
        var identityId = "User"; // Must match <Actor Name="User" ...>

        Console.WriteLine("[+] Creating process...");
        await runtime.CreateInstanceAsync(schemeCode, processId); //identityId);

        var commands = await runtime.GetAvailableCommandsAsync(processId, identityId);
        var startCommand = commands.FirstOrDefault(c => c.CommandName == "Start");

        if (startCommand != null)
        {
            Console.WriteLine("[>] Starting workflow...");
            await runtime.ExecuteCommandAsync(startCommand, identityId,identityId);
            //{
            //    ProcessId = processId,
            //    CommandName = "Start",
            //    IdentityId = identityId
            //});

            Console.WriteLine("[✓] Start command executed.");
        }
        else
        {
            Console.WriteLine("[!] Start command not available.");
        }

        // Give it a few seconds if needed for plugin to send email
        await Task.Delay(2000);

        var status = await WorkflowInit.Provider.GetInstanceStatusAsync(processId);
        Console.WriteLine($"[~] Final Status: {status}");
    }
}
