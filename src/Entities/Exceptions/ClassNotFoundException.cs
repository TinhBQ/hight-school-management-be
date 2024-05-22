using Entities.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ClassNotFoundException : NotFoundException
    {
        public ClassNotFoundException(Guid classId) : base($"Lớp học {classId}.")
        {
        }
    }
}
