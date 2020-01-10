﻿using System;
using System.Threading.Tasks;

using SwedbankPay.Sdk.Transactions;

namespace SwedbankPay.Sdk.Payments.Card
{
    public class Payment
    {
        private Payment(PaymentResponseContainer<PaymentResponse> paymentResponseContainer, SwedbankPayHttpClient client)
        {
            PaymentResponse = paymentResponseContainer.PaymentResponse;
            var operations = new Operations();

            foreach (var httpOperation in paymentResponseContainer.Operations)
            {
                operations.Add(httpOperation.Rel, httpOperation);

                switch (httpOperation.Rel.Value)
                {
                    case PaymentResourceOperations.UpdatePaymentAbort:
                        operations.Update = httpOperation;
                        break;
                        
                    case PaymentResourceOperations.RedirectAuthorization:
                        operations.RedirectAuthorization = httpOperation;
                        break;

                    case PaymentResourceOperations.ViewAuthorization:
                        operations.ViewAuthorization = httpOperation;
                        break;

                    case PaymentResourceOperations.DirectAuthorization:
                        operations.DirectAuthorization = async payload => await client.SendHttpRequestAndProcessHttpResponse<AuthorizationTransactionResponse>(httpOperation.Request.AttachPayload(payload));
                        break;

                    case PaymentResourceOperations.CreateCapture:
                        operations.Capture = async payload => await client.SendHttpRequestAndProcessHttpResponse<CaptureTransactionResponse>(httpOperation.Request.AttachPayload(payload));
                        break;

                    case PaymentResourceOperations.CreateCancellation:
                        operations.Cancel = async payload => await client.SendHttpRequestAndProcessHttpResponse<CancellationTransactionResponse>(httpOperation.Request.AttachPayload(payload));
                        break;

                    case PaymentResourceOperations.CreateReversal:
                        operations.Reversal = async payload => await client.SendHttpRequestAndProcessHttpResponse<ReversalTransactionResponse>(httpOperation.Request.AttachPayload(payload));
                        break;

                    case PaymentResourceOperations.RedirectVerification:
                        operations.RedirectVerification = httpOperation;
                        break;

                    case PaymentResourceOperations.ViewVerification:
                        operations.ViewVerification = httpOperation;
                        break;

                    case PaymentResourceOperations.DirectVerification:
                        operations.DirectVerification = httpOperation;
                        break;

                    case PaymentResourceOperations.PaidPayment:
                        operations.PaidPayment = httpOperation;
                        break;
                }
            }

            Operations = operations;
        }


        public Operations Operations { get; }

        public PaymentResponse PaymentResponse { get; }


        internal static async Task<Payment> Create(PaymentRequest paymentRequest,
                                                   SwedbankPayHttpClient client,
                                                   string paymentExpand)
        {
            var url = new Uri($"/psp/creditcard/payments{paymentExpand}", UriKind.Relative);
            
            var paymentResponseContainer = await client.HttpPost<PaymentResponseContainer<PaymentResponse>>(url, paymentRequest);
            return new Payment(paymentResponseContainer, client);
        }




        internal static async Task<Payment> Get(Uri id, SwedbankPayHttpClient client, string paymentExpand)
        {
            var url = !string.IsNullOrWhiteSpace(paymentExpand)
                ? new Uri(id.OriginalString + paymentExpand, UriKind.RelativeOrAbsolute)
                : id;

            var paymentResponseContainer = await client.HttpGet<PaymentResponseContainer<PaymentResponse>>(url);
            return new Payment(paymentResponseContainer, client); 
        }
    }
}