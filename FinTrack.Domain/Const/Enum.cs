using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FinTrack.Domain.Const
{
    public class Enum
    {
        public enum Environment
        {
            TEST,
            PROD
        }

        //veri giriş tipi. gerçekleşen veya taslak/geleceğe yönelik.
        //geçmişe yönelik veri girişi için Actual olmak zorunda, geçmişe yönelik veri girişi taslak olamaz.
        public enum EntryState
        {
            Actual = 1,
            DraftProjected = 2
        }

        public enum LedgerDirection
        {
            Inflow = 1,
            Outflow = 2
        }

        public enum PaymentChannel
        {
            Cash = 1,
            BankTransfer = 2,
            CreditCard = 3,
            MealCard = 4
        }

        public enum AssetType
        {
            BankAccount = 1,
            CreditCard = 2,
            MealCard = 3
        }

        public enum FinancialAccountType
        {
            [Description("Kredi Kartı")]
            CreditCard = 1,

            [Description("Banka Hesabı")]
            BankAccount = 2,

            [Description("Yemek / Ticket Kartı")]
            MealOrTicketCard = 3
        }

        public enum AccountType
        {
            CreditCard = 1,
            BankAccount = 2,
            MealCard = 3
        }

        public enum TransactionType
        {
            Income = 1,
            Expense = 2,
            Transfer = 3
        }

        public enum StatusType
        {
            DraftProjected = 1,
            Actual = 2,
            Cancelled = 3
        }

        public enum SettlementStatus
        {
            Open = 1,
            PartiallySettled = 2,
            Settled = 3,
            WrittenOff = 4
        }

        public enum ExpenseKind
        {
            Normal = 1,
            ProxyAdvance = 2 // "Emanet" purchase for external person
        }

        public enum ReceivableStatus
        {
            Open = 1,
            PartiallyCollected = 2,
            Closed = 3,
            WrittenOff = 4
        }
    }
}
