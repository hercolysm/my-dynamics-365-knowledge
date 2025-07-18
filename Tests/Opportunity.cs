namespace Tests
{
    [TestClass]
    public sealed class Opportunity
    {
        [TestMethod]
        public void WinOpportunityRequestTest()
        {
            Guid opportunityId = new Guid("{C2986E6E-3BE4-4DD1-9D40-A97AA4206199}");

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
        public void UpdateOpportunityStatecodeTest()
        {
            Entity opportunity = new Entity("opportunity", new Guid("{C2986E6E-3BE4-4DD1-9D40-A97AA4206199}"));
            opportunity["statecode"] = new OptionSetValue(1); // Status - Ganho
            opportunity["statuscode"] = new OptionSetValue(860090005); // Razão do Status - Ganhos do Mês
            service.Update(opportunity);
        }
    }
}
