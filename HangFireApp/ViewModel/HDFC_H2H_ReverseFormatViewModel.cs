using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireApp.ViewModel
{
    public class HDFC_H2H_ReverseFormatViewModel
    {
        public string TransactionType { get; set; }
        public string BeneficiaryCode { get; set; }
        public string BeneficiaryName { get; set; }
        public decimal InstrumentAmount { get; set; }
        public string ChequeNumber { get; set; }
        public string TransactionDate { get; set; }
        public string CustomerReferenceNo { get; set; }
        public string PaymentDetails1 { get; set; }
        public string PaymentDetails2 { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string BankRefNo { get; set; }
        public string TransactionStatus { get; set; }
        public string RejectReason { get; set; }
        public string IFCCode { get; set; }
        public string MicrCode { get; set; }
        public string UTRNumForRTGS { get; set; }
    }
}
