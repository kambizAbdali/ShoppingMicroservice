using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Features.Orders.DTOs
{
    public class OrderResponse
    {
        // Unique identifier for the entity
        public long Id { get; set; }

        /// <summary>
        /// نام کاربری خریدار
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// مبلغ نهایی سفارش
        /// </summary>
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// نام مشتری
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی مشتری
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// آدرس ایمیل مشتری
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// آدرس کامل ارسال
        /// </summary>
        public string? AddressLine { get; set; }

        /// <summary>
        /// استان یا ایالت
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// روش پرداخت سفارش
        /// </summary>
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
