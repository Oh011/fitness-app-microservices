
using AuthenticationService.Application.Abstractions.Bcakground;
using Hangfire;
using System.Linq.Expressions;

namespace AuthenticationService.Infrastructure.Background
{
    internal class HangfireBackgroundJobQueue : IBackgroundJobQueue
    {

        private readonly IBackgroundJobClient backgroundJobClient;


        public HangfireBackgroundJobQueue(IBackgroundJobClient backgroundJobClient)
        {
            this.backgroundJobClient = backgroundJobClient;
        }
        public void Enqueue<TService>(Expression<Func<TService, Task>> methodCall) where TService : class
        {

            backgroundJobClient.Enqueue<TService>(methodCall);
       

        }
    }
}
