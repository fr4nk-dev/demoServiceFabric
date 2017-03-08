using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using System.Web.Http;
using KeyWordSearchApi.Controllers.models;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using SearchActor.Interfaces;

namespace KeyWordSearchApi.Controllers
{
    [ServiceRequestActionFilter]
    public class KeywordController : ApiController
    {
        [HttpPost]
        [Route("search")]
        public async Task<KeywordSearchViewModel> SearchKeyword(KeywordSearchPostModel model)
        {
            var context = FabricRuntime.GetActivationContext();
            var config = context.GetConfigurationPackageObject("Config");
            var section = config.Settings.Sections["ActorCommunicationSettings"];
            var actorServiceName = section.Parameters["ServiceName"].Value;

            var serviceUri = new Uri($"{context.ApplicationName}/{actorServiceName}");

            var viewModel = new KeywordSearchViewModel();
            viewModel.Keyword = model.Keyword;
            if (ModelState.IsValid && model != null && model.Uris != null)
            {
                var resultTasks = new List<Task<Tuple<string, int>>>();

                foreach (var stringUri in model.Uris)
                {
                    var uri = new Uri(stringUri);
                    var actorInstance = ActorProxy.Create<ISearchActor>(ActorId.CreateRandom(), serviceUri);

                    resultTasks.Add(actorInstance.SearchForKeyword(uri, model.Keyword));

                }
                await Task.WhenAll(resultTasks);

                foreach (var t in resultTasks)
                {
                    viewModel.TotalCount += t.Result.Item2;
                    viewModel.HitsByUri.Add(new Tuple<string, int>(t.Result.Item1, t.Result.Item2));
                }
            }
            return viewModel;
        }
    }
}
