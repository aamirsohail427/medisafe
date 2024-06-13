using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;
using EHealthCare.Models;
using System.Threading.Tasks;

namespace EHealthCare.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Charge(string stripeEmail, string stripeToken, int appointid)
        {
            var customers = new SubscriptionService();
            var charges = new ChargeService();

            var customer = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            };

            var service = new ChargeService();


            var options = new ChargeCreateOptions
            {
                Amount = 2000,
                Currency = "usd",
                Description = "Charge for jenny.rosen@example.com",
                Source = stripeToken // obtained with Stripe.js,
            };
            Charge charge = service.Create(options);

            // further application specific code goes here
            GenericCrud<AppointmentPayment> apppayment = new GenericCrud<AppointmentPayment>();
            AppointmentPayment payment = new AppointmentPayment();
            payment.Amount = Convert.ToInt32(charge.Amount);
            payment.AppointmentID = appointid;
            payment.Dated = DateTime.Now;
            payment.TransactionID = charge.TransferId;
            payment.UserID =Convert.ToInt32(Session["UserId"]);
            var addnewpayment=await apppayment.CreateAsync(payment);

            return View();
        }
    }
}