namespace HouseRentingSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// This is custom user class that works with the default ASP.NET Core Identity.
    /// You can add additional info to the built-in users.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
            this.RentedHouses = new HashSet<House>();
        }

        public virtual ICollection<House> RentedHouses { get; set; }
    }
}
