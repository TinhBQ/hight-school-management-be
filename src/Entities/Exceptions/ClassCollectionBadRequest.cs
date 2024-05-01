using DomainModel.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class ClassCollectionBadRequest : BadRequestException
    {
        public ClassCollectionBadRequest() : base("Class collection được gửi từ người dùng hàng là rỗng.")
        {
        }
    }
}
