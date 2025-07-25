using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace Tests.Plugins
{
    [TestClass]
    public class OpportunityTest : MSTestSettings
    {
        [TestMethod]
        [DataRow("")]
        public void WinOpportunityRequestTest(string id)
        {
            Guid opportunityId = new Guid(id);

            var request = new WinOpportunityRequest
            {
                OpportunityClose = new Entity("opportunityclose")
                {
                    Attributes = {
                        {
                            "opportunityid",
                            new EntityReference("opportunity", opportunityId)
                        }
                        // Add additional opportunityclose column 
                        // data to provide more details.
                    }
                },
                // Won is the default and only status.
                // Custom opportunity status options
                // can be created that also represent different types
                // of win.
                Status = new OptionSetValue(3) //Won
            };

            service.Execute(request);
        }

        [TestMethod]
        [DataRow("")]
        public void UpdateOpportunityStatecodeTest(string id)
        {
            Entity opportunity = new Entity("opportunity", new Guid(id));
            opportunity["statecode"] = new OptionSetValue(1); // Status - Ganho
            opportunity["statuscode"] = new OptionSetValue(860090005); // Razão do Status - Ganhos do Mês
            service.Update(opportunity);
        }
    }
}
