
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record RegisterUserCommand:IRequest<Result>
    {

        public string Email { get; init; }
        public string Password { get; init; }


        public string FisrtName { get; init; }


        public string LastName { get; init; }   


        public string PhonenUmber { get; init; }
  


        public string UserName { get; init; }



    }
}
