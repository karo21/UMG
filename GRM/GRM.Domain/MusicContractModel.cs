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
    public class AppConstants
    {
        public const string PipeDelimiter = "|";
        public const string format = "MM-dd-yyyy";
    }

    public enum MusicContractKeys
    {
        Artist = 0,
        Title,
        Usages,
        StartDate,
        EndDate
    }

    public class MusicContractModel
    {
        public string Artist { set; get; }
        public string Title { set; get; }
        public string Usages { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }

        public string ToString(Dictionary<MusicContractKeys, int> DicolumnsSizec = null)
        {
            if (DicolumnsSizec == null)
            {
                return string.Format("| {0} | {1} | {2} | {3} | {4} |",
                Artist, Title, Usages, StartDate.HasValue ? StartDate.Value.ToString(AppConstants.format) : "", EndDate.HasValue ? EndDate.Value.ToString(AppConstants.format) : "");
            }
            else
            {

                return string.Format("| {0,-" + DicolumnsSizec[MusicContractKeys.Artist] + "} | {1,-"
                    + DicolumnsSizec[MusicContractKeys.Title] + "} | {2,-" + DicolumnsSizec[MusicContractKeys.Usages] + "} | {3,-"
                    + DicolumnsSizec[MusicContractKeys.StartDate] + "} | {4,-" + DicolumnsSizec[MusicContractKeys.EndDate] + "} |",
                    Artist, Title, Usages, StartDate.HasValue ? StartDate.Value.ToString(AppConstants.format) : "", EndDate.HasValue ? EndDate.Value.ToString(AppConstants.format) : ""
                    );

            }
        }
    }


   
}
