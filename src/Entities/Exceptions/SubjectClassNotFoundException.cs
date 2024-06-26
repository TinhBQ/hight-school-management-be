using Entities.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class SubjectClassNotFoundException : NotFoundException
    {
        public SubjectClassNotFoundException(Guid id) : base($"Phân công môn - lớp {id}.")
        {
        }
    }
}
