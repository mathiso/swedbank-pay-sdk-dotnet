﻿#region License

// --------------------------------------------------
// Copyright © Swedbank Pay. All Rights Reserved.
// 
// This software is proprietary information of Swedbank Pay.
// USE IS SUBJECT TO LICENSE TERMS.
// --------------------------------------------------

#endregion

using System.Collections.Generic;

using SwedbankPay.Sdk.Payments;
using SwedbankPay.Sdk.Transactions;

namespace SwedbankPay.Sdk.PaymentOrders
{
    public class Operations : Dictionary<LinkRelation, HttpOperation>
    {
        public HttpOperation this[LinkRelation rel] => ContainsKey(rel) ? base[rel] : null;
        public ExecuteWrapper<PaymentOrderResponseContainer> Abort { get; internal set; }
        public ExecuteRequestWrapper<TransactionRequestContainer<TransactionRequest>, CancellationTransactionResponseContainer> Cancel { get; internal set; }
        public ExecuteRequestWrapper<TransactionRequestContainer<TransactionRequest>, CaptureTransactionResponseContainer> Capture { get; internal set; }
        public ExecuteRequestWrapper<TransactionRequestContainer<TransactionRequest>, ReversalTransactionResponseContainer> Reversal { get; internal set; }
        public ExecuteRequestWrapper<PaymentOrderUpdateRequestContainer, PaymentOrderResponseContainer> Update { get; internal set; }
        public HttpOperation View { get; internal set; }
    }
}