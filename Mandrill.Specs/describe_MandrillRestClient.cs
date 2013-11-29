using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill.Models;
using NSpec;

namespace Mandrill.Specs
{
    class describe_MandrillRestClient : nspec
    {
        MandrillRestClient client;
        string emailId;
        dynamic result;
        dynamic requestObject;

        string apiKey = ConfigurationManager.AppSettings.Get("MandrillApiKey");
        string templateName = ConfigurationManager.AppSettings.Get("ValidEmailTemplate");
        string fromAddress = ConfigurationManager.AppSettings.Get("ValidFromAddress");
        string existingEmailid = ConfigurationManager.AppSettings.Get("402c1f41c10a4997a8d92d34329641eb");

        void before_each()
        {
            client = new MandrillRestClient(apiKey);
        }

        void message_send_template()
        {
            act = () => result = client.SendTemplate(requestObject);
            context["given valid email message is set"] = () =>
            {
                before = () =>
                {
                    requestObject = new RequestObject(new
                    {
                        template_name = templateName,
                        template_content = new List<RequestObject>(),
                        message = new RequestObject(new
                        {
                            from_email = fromAddress,
                            to = new List<RequestObject> { 
                                new RequestObject(new
                                {
                                    email = "foo@bar.com",
                                    name = "John Doe",
                                    type = "to"
                                })
                            }
                        })
                    });
                };
                it["sends email"] = () =>
                {
                    var firstResult = (result as IEnumerable<dynamic>).First();
                    ((string)firstResult.status).should_be("sent");
                };
            };
        }

        void message_info()
        {
            act = () => result = client.Info(emailId);

            context["given valid email exists"] = () =>
            {
                before = () =>
                {
                    emailId = existingEmailid;
                };

                it["returns email info"] = () => ((string)result._id).should_be(emailId);
            };

            context["given email does not exist"] = () =>
            {
                before = () =>
                {
                    emailId = "asdf123456";
                };
                it["returns Unknown_Message error"] = () =>
                {
                    ((string)result.status).should_be("error");
                    ((string)result.name).should_be("Unknown_Message");
                };
            };
        }
    }
}
