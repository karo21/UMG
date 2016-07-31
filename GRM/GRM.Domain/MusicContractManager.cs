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
    public class MusicContractManager
    {
        #region Private Fields
        private static volatile MusicContractManager _instance;
        private readonly List<MusicContractModel> _musicContracts;
        private static object _syncRoot = new Object();
        private const string _musicContractsFile = @"Data\MusicContracts.txt";
        #endregion

        #region Public Properties
        public static MusicContractManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new MusicContractManager();
                    }
                }

                return _instance;
            }
        }

        public Dictionary<MusicContractKeys, int> columnsSizes;
        #endregion

        #region Private Methods/.Ctor
        private MusicContractManager()
        {
            _musicContracts = new List<MusicContractModel>();

            if (File.Exists(_musicContractsFile))
            {
                using (StreamReader origFile = File.OpenText(_musicContractsFile))
                {
                    var data = origFile.ReadToEnd();
                    LoadData(data);
                }
            }
            else
            {
                //todo:
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
                                var musicContractModel = new MusicContractModel();

                                if (fields.Length > (int)MusicContractKeys.Artist)
                                {
                                    //if Artist is empty , we can log an error or warning
                                    musicContractModel.Artist = fields[(int)MusicContractKeys.Artist].Trim();
                                }

                                if (fields.Length > (int)MusicContractKeys.Title)
                                {
                                    //if Title  is empty , we can log an error or warning 
                                    musicContractModel.Title = fields[(int)MusicContractKeys.Title].Trim();
                                }

                                if (fields.Length > (int)MusicContractKeys.Usages)
                                {
                                    //if Usages  is empty , we can log an error or warning 
                                    musicContractModel.Usages = fields[(int)MusicContractKeys.Usages].Trim();
                                }

                                if (fields.Length > (int)MusicContractKeys.StartDate)
                                {
                                    DateTime startDate;

                                    //if start date has invalid format, we can log an error or warning 
                                    if (DateTime.TryParseExact(fields[(int)MusicContractKeys.StartDate].Replace(" ", ""), AppConstants.format,
                                    new CultureInfo("en-US"),
                                    DateTimeStyles.None,
                                    out startDate))
                                    {
                                        musicContractModel.StartDate = startDate;
                                    }


                                }

                                if (fields.Length > (int)MusicContractKeys.EndDate)
                                {
                                    DateTime endDate;

                                    //if end date has invalid format, we can log an error or warning
                                    if (DateTime.TryParseExact(fields[(int)MusicContractKeys.EndDate].Replace(" ", ""), AppConstants.format,
                                    new CultureInfo("en-US"),
                                    DateTimeStyles.None,
                                    out endDate))
                                    {
                                        musicContractModel.EndDate = endDate;
                                    }

                                }

                                _musicContracts.Add(musicContractModel);
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
        public List<MusicContractModel> GetActiveMusicContracts(DistPartnerContractModel partner, DateTime date)
        {

            return _musicContracts.Where(x => x.Usages.Contains(partner.Usage)
                &&
               (!x.StartDate.HasValue || x.StartDate <= date) && (!x.EndDate.HasValue || date <= x.EndDate)).
               Select(x => new MusicContractModel
               {
                   Artist = x.Artist,
                   Title = x.Title,
                   Usages = partner.Usage,
                   StartDate = x.StartDate,
                   EndDate = x.EndDate
               }).ToList();
        }
        #endregion
    }
}
