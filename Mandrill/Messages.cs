using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill.Models;
using RestSharp;
using RestSharp.Validation;

namespace Mandrill
{
    public partial class MandrillRestClient
    {
        public dynamic Info(string id)
        {
            var request = new RestRequest { Resource = "/messages/info.json" };
            dynamic requestData = new RequestObject(new { id });

            return Execute<dynamic>(request, requestData);
        }

        public virtual dynamic SendTemplate(dynamic emailMessage)
        {
            var request = new RestRequest { Resource = "/messages/send-template.json" };

            return Execute<dynamic>(request, emailMessage);
        }
    }
}
