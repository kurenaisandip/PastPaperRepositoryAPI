using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Services.Payments;
using PastPaperRepository.Contracts.Requests.Payments;
using Stripe.Checkout;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class PaymentController: ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost(ApiEndPoints.Payments.CreatePayment)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request, CancellationToken token)
    {
        var model = request.MapToCreatePaymentModel();
        var result = await _paymentService.CreatePaymentAsync(model, token);

        return Ok(result);
    }  
    
    [AllowAnonymous]
    [HttpPost(ApiEndPoints.Payments.CheckoutSession)]
    public async Task<IActionResult> CheckoutSession([FromBody] CreatePaymentRequest request, CancellationToken token)
    {
        // Map the request to the payment model
        var model = request.MapToCreatePaymentModel();
        var unitAmount = (long)(model.Price * 100);

        // Define success and cancel URLs
        var successUrl = $"http://127.0.0.1:5500/success.html";
        var cancelUrl = $"http://127.0.0.1:5500/cancel.html";

        // Create session options
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "payment",
            SuccessUrl = successUrl, // Correctly placed success URL
            CancelUrl = cancelUrl,   // Correctly placed cancel URL
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "npr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Thank You for buying our {model.ProductName}",
                            Description = $"Your subscription will work until {model.ValidUntil:yyyy-MM-dd}",
                        },
                        UnitAmount = unitAmount,
                    },
                    Quantity = 1,
                }
            },
        };

        // Use Stripe's session service to create the session
        var service = new SessionService();
        Session session = await service.CreateAsync(options, cancellationToken: token);

        return new JsonResult(new { sessionId = session.Id });
        // Set the session URL in the response headers
        // Response.Headers.Add("Location", session.Url);
        //
        // // Return a 303 See Other response
        // return new StatusCodeResult(303);
    }

}