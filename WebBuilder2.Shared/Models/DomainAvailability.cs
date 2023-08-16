using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBuilder2.Shared.Models;

public enum DomainAvailability
{
    Available,
    AvailableReserved,
    AvailablePreOrder,
    DontKnow,
    Pending,
    Reserved,
    Unavailable,
    UnavailablePremium,
    UnavailableRestricted
}
