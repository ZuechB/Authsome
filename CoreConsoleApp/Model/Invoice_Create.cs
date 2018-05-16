using System;

namespace CoreConsoleApp.Model
{
    public class Invoice_Create
    {
        //public int Deposit { get; set; }
        //public string domain { get; set; }
        //public bool sparse { get; set; }
        public string Id { get; set; }
        //public string SyncToken { get; set; }
        //public Invoice_Create_Metadata MetaData { get; set; }
        //public Invoice_Create_Customfield[] CustomField { get; set; }
        //public string DocNumber { get; set; }
        //public string TxnDate { get; set; }
        //public Invoice_Create_Linkedtxn[] LinkedTxn { get; set; }
        public Invoice_Create_Line[] Line { get; set; }
        //public Invoice_Create_Txntaxdetail TxnTaxDetail { get; set; }
        public Invoice_Create_Customerref CustomerRef { get; set; }
        public Invoice_Create_Customermemo CustomerMemo { get; set; }
        public Invoice_Create_Billaddr BillAddr { get; set; }
        public Invoice_Create_Shipaddr ShipAddr { get; set; }
        //public Invoice_Create_Salestermref SalesTermRef { get; set; }
        //public string DueDate { get; set; }
        //public float TotalAmt { get; set; }
        //public bool ApplyTaxAfterDiscount { get; set; }
        //public string PrintStatus { get; set; }
        //public string EmailStatus { get; set; }
        public Invoice_Create_Billemail BillEmail { get; set; }
        public bool AllowOnlineACHPayment { get; set; }
        //public float Balance { get; set; }
    }

    public class Invoice_Create_Metadata
    {
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }

    public class Invoice_Create_Txntaxdetail
    {
        public Invoice_Create_Txntaxcoderef TxnTaxCodeRef { get; set; }
        public float TotalTax { get; set; }
        public Invoice_Create_Taxline[] TaxLine { get; set; }
    }

    public class Invoice_Create_Txntaxcoderef
    {
        public string value { get; set; }
    }

    public class Invoice_Create_Taxline
    {
        public float Amount { get; set; }
        public string DetailType { get; set; }
        public Invoice_Create_Taxlinedetail TaxLineDetail { get; set; }
    }

    public class Invoice_Create_Taxlinedetail
    {
        public Invoice_Create_Taxrateref TaxRateRef { get; set; }
        public bool PercentBased { get; set; }
        public int TaxPercent { get; set; }
        public float NetAmountTaxable { get; set; }
    }

    public class Invoice_Create_Taxrateref
    {
        public string value { get; set; }
    }

    public class Invoice_Create_Customerref
    {
        public string value { get; set; }
        public string name { get; set; }
    }

    public class Invoice_Create_Customermemo
    {
        public string value { get; set; }
    }

    public class Invoice_Create_Billaddr
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }

    public class Invoice_Create_Shipaddr
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
        public string City { get; set; }
        public string CountrySubDivisionCode { get; set; }
        public string PostalCode { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }

    public class Invoice_Create_Salestermref
    {
        public string value { get; set; }
    }

    public class Invoice_Create_Billemail
    {
        public string Address { get; set; }
    }

    public class Invoice_Create_Customfield
    {
        public string DefinitionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string StringValue { get; set; }
    }

    public class Invoice_Create_Linkedtxn
    {
        public string TxnId { get; set; }
        public string TxnType { get; set; }
    }

    public class Invoice_Create_Line
    {
        //public string Id { get; set; }
        //public int LineNum { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
        public string DetailType { get; set; }
        public Invoice_Create_Salesitemlinedetail SalesItemLineDetail { get; set; }
        //public Invoice_Create_Subtotallinedetail SubTotalLineDetail { get; set; }
    }

    public class Invoice_Create_Salesitemlinedetail
    {
        public Invoice_Create_Itemref ItemRef { get; set; }
        public float UnitPrice { get; set; }
        public int Qty { get; set; }
        //public Invoice_Create_Taxcoderef TaxCodeRef { get; set; }
    }

    public class Invoice_Create_Itemref
    {
        public string value { get; set; }
        public string name { get; set; }
    }

    public class Invoice_Create_Taxcoderef
    {
        public string value { get; set; }
    }

    public class Invoice_Create_Subtotallinedetail
    {
    }
}
