using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AuthenticationService.Application.Abstractions.Bcakground
{
    public interface IBackgroundJobQueue
    {

        void Enqueue<TService>(Expression<Func<TService,Task>> methodcall ) where TService : class;
    }
}
