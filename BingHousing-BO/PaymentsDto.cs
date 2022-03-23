using System;
using System.Runtime.CompilerServices;

namespace BingHousing_BO
{
	public class PaymentsDto
	{
		public decimal? Amount
		{
			get;
			set;
		}

		public string BillDescription
		{
			get;
			set;
		}

		public string Css
		{
			get;
			set;
		}

		public int CustomerId
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public int? InvoiceId
		{
			get;
			set;
		}

		public bool IsPrinted
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public DateTime? PaidDate
		{
			get;
			set;
		}

		public string Payee
		{
			get;
			set;
		}

		public int PayeeId
		{
			get;
			set;
		}

		public int? PaymentId
		{
			get;
			set;
		}

		public string PaymentMode
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public PaymentsDto()
		{
		}
	}
}