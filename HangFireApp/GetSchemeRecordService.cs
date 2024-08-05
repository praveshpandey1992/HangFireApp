using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HangFireApp
{
    public class GetSchemeRecordService : IGetSchemeRecord
    {
        public bool GetSchemeRecordWithFreezeId()
        {
            try
            {
                //2. Call Scheme API
                //string apiUrl = "http://localhost:49908/HDFC_HostToHost_WelfareSchemeController";

                //var client = new RestClient("http://localhost:49908/HDFC_HostToHost_WelfareScheme/PrepareCSVFileOfWelfareScheme");
                var client = new RestClient("https://pblabour.gov.in/elabour/HDFC_HostToHost_WelfareScheme/PrepareCSVFileOfWelfareScheme");

                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "pAX-%Lh7/G_5sK9!`'Jyfr8#`M_q,5jzBpf8fgAQq*Cp-sqY9T>^P#Pb%gxW~zkW=b33c436c-c71f-4621-8dc0-14bee8ea6552");
                var response = client.Execute(request);

                List<HDFC_H2H_CSVFormateViewModel> welfareSchemeData = new List<HDFC_H2H_CSVFormateViewModel>();
                welfareSchemeData = JsonConvert.DeserializeObject<List<HDFC_H2H_CSVFormateViewModel>>(response.Content);

                #region prepare csv with data
                if (welfareSchemeData.Count > 0)
                {
                    int randome = (new Random()).Next(100, 1000);
                    string filename = "TEST_0604RBI_0604RBI" + DateTime.Now.ToString("ddMM") + "." + randome;

                    //C:\HDFC\HDFCForward\src
                    HDFC_H2H_Operations.CreateCSV<HDFC_H2H_CSVFormateViewModel>(welfareSchemeData, @"C:\\HDFC\\HDFCForward\\src\\" + filename);
                }

                #endregion prepare csv with data
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        void IGetSchemeRecord.GetSchemeRecordWithFreezeId()
        {
            throw new NotImplementedException();
        }
    }

    public class HDFC_H2H_CSVFormateViewModel
    {
        public string TransactionType { get; set; }
        public string BeneficiaryCode { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public decimal InstrumentAmount { get; set; }
        public string BeneficiaryName { get; set; }
        public string DraweeLocation { get; set; }
        public string PrintLocation { get; set; }
        public string BeneAddress1 { get; set; }
        public string BeneAddress2 { get; set; }
        public string BeneAddress3 { get; set; }
        public string BeneAddress4 { get; set; }
        public string BeneAddress5 { get; set; }
        public Int64? InstructionReferenceNumber { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string Paymentdetails1 { get; set; }
        public string Paymentdetails2 { get; set; }
        public string Paymentdetails3 { get; set; }
        public string Paymentdetails4 { get; set; }
        public string Paymentdetails5 { get; set; }
        public string Paymentdetails6 { get; set; }
        public string Paymentdetails7 { get; set; }
        public string ChequeNumber { get; set; }

        // Must be a Today date or next day (Not a past date)
        public string TrnDate { get; set; }
        public string MICRNumber { get; set; }
        public string IFCCode { get; set; }
        public string BeneBankName { get; set; }
        public string BeneBankBranchName { get; set; }
        public string BeneficiaryEmailId { get; set; }
        public Int64 Freezid { get; set; }
    }
}
