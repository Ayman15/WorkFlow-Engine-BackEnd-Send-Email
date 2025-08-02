//using OptimaJet.Workflow.Core.Model;
//using OptimaJet.Workflow.Core.Runtime;
//using OptimaJet.Workflow.Core.Activities;

//public class ScadaRequestActivity : CodeActivity
//{
//    protected override ActivityExecutionResult Execute(ActivityExecutionContext context)
//    {
//        context.ProcessInstance.AddParameter("WeatherDate", DateTime.Now.ToString("yyyy-MM-dd"));
//        context.ProcessInstance.AddParameter("WeatherTemperature", "32°C");
//        context.ProcessInstance.AddParameter("Weather.latitude", "29.97");

//        return ActivityExecutionResult.Next();
//    }
//}
