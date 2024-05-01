using Entities.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class TeacherNotFoundException : NotFoundException
    {
        public TeacherNotFoundException(Guid teacherId) : base($"Giáo viên {teacherId}.")
        {
        }
    }
}
