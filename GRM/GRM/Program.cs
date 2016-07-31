using GRM.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM
{
    class Program
    {
        static void Main(string[] args)
        {

            var musicContractManager = MusicContractManager.Instance;
            var distPartnerContractManager = DistPartnerContractManager.Instance;
            var _grmPlatform = new GlobalRightManagmentPlatform(musicContractManager, distPartnerContractManager);
            string searchString;
            while ((searchString = Console.ReadLine()) != string.Empty)
            {
                try
                {
                    var result =  _grmPlatform.Search(searchString.Trim());

                    _grmPlatform.CalculateHeaderSizes(result);
                    Console.WriteLine(_grmPlatform.PrintHeader());

                    foreach (var contract in result)
                    {
                        Console.WriteLine(contract.ToString(_grmPlatform.ColumnsSize));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }            
            }
        }
    }
}
