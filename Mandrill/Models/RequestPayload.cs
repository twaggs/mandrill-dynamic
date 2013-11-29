using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oak;

namespace Mandrill.Models
{
    public class RequestObject : Gemini
    {
        public RequestObject(object dto) : base(dto) { }
        public RequestObject() : this(new { }) { }
    }
}
