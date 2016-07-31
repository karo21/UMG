using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Domain
{

    public class GlobalRightManagmentPlatform
    {
        #region Private Properties
        private MusicContractManager _musicContractManager;
        private DistPartnerContractManager _distPartnerContractModel; 
        #endregion

        public readonly Dictionary<MusicContractKeys, int> ColumnsSize;

        #region Public Methods/.Ctor
        public GlobalRightManagmentPlatform(MusicContractManager musicContractManager, DistPartnerContractManager distPartnerContractManager)
        {
            _musicContractManager = musicContractManager;
            _distPartnerContractModel = distPartnerContractManager;

            ColumnsSize = new Dictionary<MusicContractKeys, int>();
            ColumnsSize.Add(MusicContractKeys.Artist, MusicContractKeys.Artist.ToString().Length);
            ColumnsSize.Add(MusicContractKeys.Title, MusicContractKeys.Title.ToString().Length);
            ColumnsSize.Add(MusicContractKeys.Usages, MusicContractKeys.Usages.ToString().Length);
            ColumnsSize.Add(MusicContractKeys.StartDate, MusicContractKeys.StartDate.ToString().Length);
            ColumnsSize.Add(MusicContractKeys.EndDate, MusicContractKeys.EndDate.ToString().Length);
        }

        public List<MusicContractModel> Search(string searchString)
        {
            string partnerName;
            string effectiveDate;

            if (searchString.IndexOf(' ') == -1)
            {
                throw new Exception("Please insert partner name then effective date(MM-dd-yyyy), for ex.: partner_name 01-05-2010");
            }
            else
            {
                var lastSpaceIndex = searchString.LastIndexOf(' ');
                partnerName = searchString.Substring(0, lastSpaceIndex);
                effectiveDate = searchString.Substring(lastSpaceIndex, searchString.Length - lastSpaceIndex);

                DateTime date;
                if (!DateTime.TryParseExact(effectiveDate.Trim(), AppConstants.format,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out date))
                {
                    throw new Exception("effective date is invalid");
                }

                var partner = _distPartnerContractModel.GetPartner(partnerName);
                var result = new List<MusicContractModel>();


                if (partner == null)
                {
                    return result;
                }



                return _musicContractManager.GetActiveMusicContracts(partner, date);

            }

        }

        public void CalculateHeaderSizes(List<MusicContractModel> musicContracts)
        {
            foreach (var model in musicContracts)
            {
                if (!string.IsNullOrEmpty(model.Artist) && model.Artist.Length > ColumnsSize[MusicContractKeys.Artist])
                {
                    ColumnsSize[MusicContractKeys.Artist] = model.Artist.Length;
                }

                if (!string.IsNullOrEmpty(model.Title) && model.Title.Length > ColumnsSize[MusicContractKeys.Title])
                {
                    ColumnsSize[MusicContractKeys.Title] = model.Title.Length;
                }

                if (!string.IsNullOrEmpty(model.Usages) && model.Usages.Length > ColumnsSize[MusicContractKeys.Usages])
                {
                    ColumnsSize[MusicContractKeys.Usages] = model.Usages.Length;
                }

                if (model.StartDate.HasValue && model.StartDate.Value.ToString(AppConstants.format).Length > ColumnsSize[MusicContractKeys.StartDate])
                {
                    ColumnsSize[MusicContractKeys.StartDate] = model.StartDate.Value.ToString(AppConstants.format).Length;
                }

                if (model.EndDate.HasValue && model.EndDate.Value.ToString(AppConstants.format).Length > ColumnsSize[MusicContractKeys.EndDate])
                {
                    ColumnsSize[MusicContractKeys.EndDate] = model.EndDate.Value.ToString(AppConstants.format).Length;
                }
            }

        }

        public string PrintHeader()
        {
            return string.Format("| {0,-" + ColumnsSize[MusicContractKeys.Artist] + "} | {1,-"
                + ColumnsSize[MusicContractKeys.Title] + "} | {2,-" + ColumnsSize[MusicContractKeys.Usages] + "} | {3,-"
                + ColumnsSize[MusicContractKeys.StartDate] + "} | {4,-" + ColumnsSize[MusicContractKeys.EndDate] +"} |",
               MusicContractKeys.Artist.ToString(), MusicContractKeys.Title.ToString(), MusicContractKeys.Usages.ToString(), MusicContractKeys.StartDate.ToString(), MusicContractKeys.EndDate.ToString()
                );


        }
        #endregion
    }
}
