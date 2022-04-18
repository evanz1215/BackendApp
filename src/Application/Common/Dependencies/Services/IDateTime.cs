using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dependencies.Services
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
