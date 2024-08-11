using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models.Contracts;

public interface IApiResponse
{
    public IEnumerable<ApiError> Errors { get; set; }
}
