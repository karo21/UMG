using GRM.Domain;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace GRM.Specflow
{
    [Binding]
    public class GRMSteps
    {
        GlobalRightManagmentPlatform _grmPlatform;

        [Given(@"Global Right Managment Platform")]
        public void GivenGlobalRightManagmentPlatform()
        {
            _grmPlatform = new GlobalRightManagmentPlatform(MusicContractManager.Instance, DistPartnerContractManager.Instance);
        }
        
        [Then(@"the output should be")]
        public void ThenTheOutputShouldBe(Table table)
        {
            var result = ScenarioContext.Current["result"] as List<MusicContractModel>;
            table.CompareToSet<MusicContractModel>(result);

            ScenarioContext.Current.Remove("result");
        }
       

        [When(@"user perform search by '(.*)'")]
        public void WhenUserPerformSearchBy(string p0)
        {           
            var result = _grmPlatform.Search(p0);
            ScenarioContext.Current["result"] = result;
        }
    }
}
