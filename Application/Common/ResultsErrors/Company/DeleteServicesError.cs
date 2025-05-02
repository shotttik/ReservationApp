using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.ResultsErrors.Company
{
    internal class DeleteServicesError
    {
        public static readonly Error NotFound = Error.NotFound("DeleteServicesError.NotFound", "Company service not found.");
    }
}
