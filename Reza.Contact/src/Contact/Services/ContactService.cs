using Contact.Infrastructure;

namespace Contact.Services
{
   
        public sealed class ContactService(ContactUserDbContext context)
        {
            public ContactUserDbContext Context { get; } = context;
        }
    
    
}
