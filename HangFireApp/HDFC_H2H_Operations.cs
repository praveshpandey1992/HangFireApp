using HangFireApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HangFireApp
{
    // This methode is used to generate csv file with comma seperator by using refelections. 
    public static class HDFC_H2H_Operations
    {
        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }

        public static void CreateCSV<T>(List<T> list, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                //CreateHeader(list, sw);
                CreateRows(list, sw);
            }
        }

        public static HDFC_H2H_ReverseFormatViewModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            HDFC_H2H_ReverseFormatViewModel reverseFormat = new HDFC_H2H_ReverseFormatViewModel();
            reverseFormat.TransactionType = values[0];
            reverseFormat.BeneficiaryCode = values[1];
            reverseFormat.BeneficiaryName = values[2];
            reverseFormat.InstrumentAmount = Convert.ToDecimal(values[3]);
            reverseFormat.ChequeNumber = values[4];
            reverseFormat.TransactionDate = values[5];
            reverseFormat.CustomerReferenceNo = values[6];
            reverseFormat.PaymentDetails1 = values[7];
            reverseFormat.PaymentDetails2 = values[8];
            reverseFormat.BeneficiaryAccountNumber = values[9];
            reverseFormat.BankRefNo = values[10];
            reverseFormat.TransactionStatus = values[11];
            reverseFormat.RejectReason = values[12];
            reverseFormat.IFCCode = values[13];
            reverseFormat.MicrCode = values[14];
            reverseFormat.UTRNumForRTGS = values[15];

            return reverseFormat;
        }
    }
}


