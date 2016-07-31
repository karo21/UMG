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
    public enum DistPartnerKeys
    {
        Partner = 0,
        Usages
    }

    public class DistPartnerContractModel
    {
        public string Partner { set; get; }
        public string Usage { set; get; }
    }
}
