using System;

namespace CoreConsoleApp.Model
{
    public class Invoice_Create_Response_Rootobject
    {
        public Invoice_Create_Response_Invoice Invoice { get; set; }
        public DateTime time { get; set; }
    }

    public class Invoice_Create_Response_Invoice
    {
        public decimal Deposit { get; set; }
        public bool AllowIPNPayment { get; set; }
        public bool AllowOnlinePayment { get; set; }
        public bool AllowOnlineCreditCardPayment { get; set; }
        public bool AllowOnlineACHPayment { get; set; }
        public string domain { get; set; }
        public bool sparse { get; set; }
        public string Id { get; set; }
        public string SyncToken { get; set; }
        public Invoice_Create_Response_Metadata MetaData { get; set; }
        public Invoice_Create_Response_Customfield[] CustomField { get; set; }
        public string DocNumber { get; set; }
        public DateTime TxnDate { get; set; }
        public Invoice_Create_Response_Currencyref CurrencyRef { get; set; }
        public QueryCustomerInvoices_Linkedtxn[] LinkedTxn { get; set; }
        public Invoice_Create_Response_Line[] Line { get; set; }
        public Invoice_Create_Response_Txntaxdetail TxnTaxDetail { get; set; }
        public Invoice_Create_Response_Customerref CustomerRef { get; set; }
        public Invoice_Create_Response_Billaddr BillAddr { get; set; }
        public Invoice_Create_Response_Shipaddr ShipAddr { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmt { get; set; }
        public bool ApplyTaxAfterDiscount { get; set; }
        public string PrintStatus { get; set; }
        public string EmailStatus { get; set; }
        public Invoice_Create_Response_Billemail BillEmail { get; set; }
        public decimal Balance { get; set; }
    }

    public class Invoice_Create_Response_Metadata
    {
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }

    public class QueryCustomerInvoices_Linkedtxn
    {
        public string TxnId { get; set; }
        public string TxnType { get; set; }
    }

    public class Invoice_Create_Response_Currencyref
    {
        public string value { get; set; }
        public string name { get; set; }
    }

    public class Invoice_Create_Response_Txntaxdetail
    {
        public decimal TotalTax { get; set; }
        //public Invoice_TaxLine TaxLine { get; set; }
    }

    public class Invoice_TaxLine
    {
        public decimal Amount { get; set; }
        public string DetailType { get; set; }
        public Invoice_TaxLineDetail TaxLineDetail { get; set; }
    }

    public class Invoice_TaxLineDetail
    {
        public Invoice_LineDetail_TaxRateRef TaxRateRef { get; set; }
        public bool PercentBased { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal NetAmountTaxable { get; set; }
    }

    public class Invoice_LineDetail_TaxRateRef
    {
        public string value { get; set; }
    }


    public class Invoice_Create_Response_Customerref
    {
        public string value { get; set; }
        public string name { get; set; }
    }

    public class Invoice_Create_Response_Billaddr
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
    }

    public class Invoice_Create_Response_Shipaddr
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
    }

    public class Invoice_Create_Response_Billemail
    {
        public string Address { get; set; }
    }

    public class Invoice_Create_Response_Customfield
    {
        public string DefinitionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Invoice_Create_Response_Line
    {
        public string Id { get; set; }
        public int LineNum { get; set; }
        public decimal Amount { get; set; }
        public string DetailType { get; set; }
        public Invoice_Create_Response_Salesitemlinedetail SalesItemLineDetail { get; set; }
        public Invoice_Create_Response_Subtotallinedetail SubTotalLineDetail { get; set; }
    }

    public class Invoice_Create_Response_Salesitemlinedetail
    {
        public Invoice_Create_Response_Itemref ItemRef { get; set; }
        public Invoice_Create_Response_Taxcoderef TaxCodeRef { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Invoice_Create_Response_Itemref
    {
        public string value { get; set; }
        public string name { get; set; }
    }

    public class Invoice_Create_Response_Taxcoderef
    {
        public string value { get; set; }
    }

    public class Invoice_Create_Response_Subtotallinedetail
    {
    }
}
