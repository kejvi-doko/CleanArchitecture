using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure;
using Amazon.Lambda.Core;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NorthwindLambda
{
    public class Function
    {
        private IMediator _mediator;
        public IConfiguration Configuration { get; }

        public Function(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app.
            _mediator = serviceProvider.GetService<IMediator>();
            Configuration = configuration;
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {

            serviceCollection.AddApplication();
            serviceCollection.AddInfrastructure(Configuration);

            serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();

        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<TodosVm> FunctionHandler(object input, ILambdaContext context)
        {
            return await _mediator.Send(new GetTodosQuery());
        }
    }
}
