using EventHorizon.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHorizon.Application.UseCases.Interfaces
{
    public interface ILoginUseCase
    {
        Task<(string, string)> ExecuteAsync(RegsiterUserRequest request);
    }
}
