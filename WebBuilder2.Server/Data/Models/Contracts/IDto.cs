using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Data.Models.Contracts;

public interface IDto<T> where T : class
{
    public T FromDto();
}
