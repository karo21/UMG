using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Domain
{
    public class DistPartnerContractManager
    {
        #region Private Fields
        private static volatile DistPartnerContractManager _instance;
        private readonly List<DistPartnerContractModel> _distributionPartnerContracts;
        private static object _syncRoot = new Object();
        private const string _distributionPartnerContractsPath = @"Data\DistributionPartnerContracts.txt";

        #endregion

        #region Public Properties
        public static DistPartnerContractManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new DistPartnerContractManager();
                    }
                }

                return _instance;
            }
        }
        #endregion

        #region Private Methods/.Ctor
        private DistPartnerContractManager()
        {
            _distributionPartnerContracts = new List<DistPartnerContractModel>();
            if (File.Exists(_distributionPartnerContractsPath))
            {
                using (StreamReader origFile = File.OpenText(_distributionPartnerContractsPath))
                {
                    var data = origFile.ReadToEnd();
                    LoadData(data);
                }
            }
            else
            {
                //todo: error
            }


        }

        private void LoadData(string data)
        {

            TextReader reader = new StringReader(data);
            try
            {
                string[] fields;

                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    parser.Delimiters = new[] { AppConstants.PipeDelimiter };

                    while (!parser.EndOfData)
                    {
                        try
                        {
                            var sb = new StringBuilder();

                            if ((fields = parser.ReadFields()) != null && fields.Length > 0)
                            {
                                var partnerContractModel = new DistPartnerContractModel();

                                if (fields.Length > (int)DistPartnerKeys.Partner)
                                {
                                    //if Partner is empty , we can log an error or warning
                                    partnerContractModel.Partner = fields[(int)DistPartnerKeys.Partner].Trim();
                                }

                                if (fields.Length > (int)DistPartnerKeys.Usages)
                                {
                                    //if Usages  is empty , we can log an error or warning 
                                    partnerContractModel.Usage = fields[(int)DistPartnerKeys.Usages].Trim();
                                }

                                _distributionPartnerContracts.Add(partnerContractModel);
                            }
                        }
                        catch (MalformedLineException ex)
                        {
                            //todo: Log errors
                        }
                    }

                }
            }
            catch (Exception ex0)
            {
                //todo: Log errors
            }
            finally
            {
                reader.Close();

            }
        }
        #endregion

        #region Public Methods
        public DistPartnerContractModel GetPartner(string partnerName)
        {
            return _distributionPartnerContracts.FirstOrDefault(x => string.Equals(x.Partner, partnerName, StringComparison.CurrentCultureIgnoreCase));
        }
        #endregion
    }
}
