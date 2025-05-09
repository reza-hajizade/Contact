using System.Security.Policy;
using System.Xml.Serialization;
using Contact.EndPoints.Contracts;
using Contact.Model;
using Contact.Services;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using UserInfo.Infrastructure.Consumer.IntegrationEvents;

namespace Contact.EndPoints
{
    public static class ContactEndpoints
    {
        

        // Maps all contact-related endpoints (create, update, delete, get) to the application.       
        public static IEndpointRouteBuilder MapContactEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/create/{userId}", CreateContact);
            app.MapPut("/update/{PhoneNumber}", UpdateContact);
            app.MapDelete("/delete", DeleteContact);
            app.MapGet("/get", GetContact);

            return app;
        }


        public static async Task<IResult> CreateContact(
            [AsParameters] ContactService services,
             CreateContactRequest request,
             IValidator<CreateContactRequest> validator,
             IPublishEndpoint publisher,
             [FromRoute(Name = "userId")] int userId)
        {
            var validate = await validator.ValidateAsync(request);
            if (!validate.IsValid)
            {
                var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }


            var existingContact=await services.Context.contactUsers
                .SingleOrDefaultAsync(c => c.PhoneNumber == request.phoneNumber);

            if (existingContact != null)
            {
                return Results.BadRequest("This PhoneNumber Already Exist");
            }

            var newContact=ContactUser.Create(
                request.phoneNumber,
                request.email
            );

            if(newContact == null)
            {
                return Results.BadRequest("Error in creating contact");
            }

            services.Context.contactUsers.Add(newContact);
            await services.Context.SaveChangesAsync();

            // Publish an integration event to notify other services that a new contact has been created for a user

            await publisher.Publish(new ContactAddedEvent(newContact.PhoneNumber, userId, DateTime.Now));
            
            return Results.Ok(newContact);
        }

        public static async Task<IResult> GetContact(
            [AsParameters] ContactService service
            )
        {
            var contacts = await service.Context.contactUsers
                .OrderBy(x => x.Id)
                .Select(x => new GetContactResponse(x.PhoneNumber, x.Email))
                .ToListAsync();
                
                return Results.Ok(contacts);    

        }


        public static async Task<IResult> UpdateContact(
            [AsParameters] ContactService service,
             string PhoneNumber,
            UpdateContactRequest Request,            
            IValidator<UpdateContactRequest> validator)
        {
            var validate = await validator.ValidateAsync(Request);
            if(validate.IsValid)
            {
                var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var Contact = await service.Context.contactUsers
                .SingleOrDefaultAsync(x => x.PhoneNumber == PhoneNumber);
           
            if (Contact is null)           
                return Results.NotFound("Contact not found");
            
            var isDuplicate=await service.Context.contactUsers
                .AnyAsync(x => x.PhoneNumber == Request.PhoneNumber);

            if(isDuplicate)
                return Results.BadRequest("This PhoneNumber Already Exist");


            Contact.Update(Request.PhoneNumber, Request.Email);
            await service.Context.SaveChangesAsync();

            return Results.Ok("Contact updated successfully");

        }


        public static async Task<IResult> DeleteContact(
            [AsParameters] ContactService services,
            string PhoneNumber
            )
        {

            var contact = await services.Context.contactUsers.SingleOrDefaultAsync(x=>x.PhoneNumber==PhoneNumber);
            if (contact is null)
            {
                return Results.NotFound("User not found");
            }
            services.Context.contactUsers.Remove(contact);
            await services.Context.SaveChangesAsync();
            return Results.Ok("User deleted successfully");
        }

    }
}
