namespace Contact.Model;

    public class ContactUser
    {
    public int Id { get; set; }
    public string PhoneNumber { get;private set; }
    public string Email { get;private set; }

     


    public static ContactUser Create( string phoneNumber, string email)
    {
        return new ContactUser
        {
        
            PhoneNumber = phoneNumber,
            Email = email
        };
    }

    public void Update(string phoneNumber, string email)
    {
        PhoneNumber = phoneNumber;
        Email = email;
    }

}




