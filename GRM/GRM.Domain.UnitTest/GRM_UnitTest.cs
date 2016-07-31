using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GRM.Domain.UnitTest
{
    [TestClass]
    public class GRM_UnitTest
    {
        private GlobalRightManagmentPlatform _grmPlatform;
        private DistPartnerContractManager _distPartnerContractManager;
        private MusicContractManager _musicContractManager;

        [TestInitialize]
        public void Initialize()
        {
            _musicContractManager = MusicContractManager.Instance;
            _distPartnerContractManager = DistPartnerContractManager.Instance;
            _grmPlatform = new GlobalRightManagmentPlatform(_musicContractManager, _distPartnerContractManager);
        }

        [TestMethod]
        [Description("Domain_GlobalRightManagmentPlatform.GetActiveMusicContracts() - NoEffectiveDate")]
        [ExpectedException(typeof(Exception))]
        [TestCategory("GlobalRightManagmentPlatform")]
        public void GRM_GetActiveMusicContracts_NoEffectiveDate_ThrowException()
        {
            try
            {
                // This line should throw an exception
                _grmPlatform.Search("InvalidText");
            }
            catch (Exception exc)
            {
                Assert.AreEqual("Please insert partner name then effective date(MM-dd-yyyy), for ex.: partner_name 01-05-2010", exc.Message);
                throw;
            }
        }

        [TestMethod]
        [Description("Domain_GlobalRightManagmentPlatform.GetActiveMusicContracts() - NoPartnerName and NoEffectiveDate")]
        [ExpectedException(typeof(Exception))]
        [TestCategory("GlobalRightManagmentPlatform")]
        public void GRM_GetActiveMusicContracts_NoPartnerName_NoEffectiveDate_ThrowException()
        {
            try
            {
                // This line should throw an exception
                _grmPlatform.Search("InvalidText");
            }
            catch (Exception exc)
            {
                Assert.AreEqual("Please insert partner name then effective date(MM-dd-yyyy), for ex.: partner_name 01-05-2010", exc.Message);
                throw;
            }
        }

        [TestMethod]
        [Description("Domain_GlobalRightManagmentPlatform.GetActiveMusicContracts() - InvalidEffectiveDate")]
        [ExpectedException(typeof(Exception))]
        [TestCategory("GlobalRightManagmentPlatform")]
        public void GRM_GetActiveMusicContracts_InvalidEffectiveDate_ThrowException()
        {
            try
            {
                // This line should throw an exception
                _grmPlatform.Search("itunes 03-01.2012");
            }
            catch (Exception exc)
            {
                Assert.AreEqual("effective date is invalid", exc.Message);
                throw;
            }
        }

        [TestMethod]
        [Description("Domain_MusicContractManager_GetMusicContracts")]
        [TestCategory("MusicContractManager")]
        public void MusicContractManager_GetMusicContracts()
        {
            PrivateObject accessor = new PrivateObject(_musicContractManager);
            List<MusicContractModel> accessiblePrivateField = (List<MusicContractModel>)accessor.GetField("_musicContracts");
            Assert.IsTrue(accessiblePrivateField.Any(), "Music Contract data is empty");
        }

        [TestMethod]
        [Description("Domain_DistPartnerContractManager_GetPartner")]
        [TestCategory("DistPartnerContractManager")]
        public void DistPartnerContractManager_GetPartner()
        {
            PrivateObject accessor = new PrivateObject(_distPartnerContractManager);
            List<DistPartnerContractModel> accessiblePrivateField = (List<DistPartnerContractModel>)accessor.GetField("_distributionPartnerContracts");
            Assert.IsTrue(accessiblePrivateField.Any(), "DistributionPartnerContracts data is empty");
        }
    }
}
