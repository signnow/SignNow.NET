using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class UserFaker : Faker<User>
    {
        /// <summary>
        /// Faker <see cref="User"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "active": false,
        ///   "first_name": "Jesus",
        ///   "last_name": "Monahan",
        ///   "primary_email": "Jesus_Monahan@gmail.com"
        /// }
        /// </code>
        /// </example>
        public UserFaker()
        {
            Rules((f, o) => {
                o.Active      = f.Random.Bool();
                o.FirstName   = f.Name.FirstName();
                o.LastName    = f.Name.LastName();
                o.Email       = f.Internet.Email(o.FirstName, o.LastName);
            });
        }
    }

    public class UserSignNowFaker : Faker<User>
    {
        /// <summary>
        /// Faker <see cref="User"/> with signnow  email.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "active": true,
        ///   "first_name": "Jesus",
        ///   "last_name": "Monahan",
        ///   "primary_email": "signnow.tutorial+Jesus_Monahan@gmail.com"
        /// }
        /// </code>
        /// </example>
        public UserSignNowFaker()
        {
            Rules((f, o) => {
                o.Active      = true;
                o.FirstName   = f.Name.FirstName();
                o.LastName    = f.Name.LastName();
                o.Email       = "signnow.tutorial+" + f.Internet.Email(o.FirstName, o.LastName, "gmail.com");
            });
        }
    }
}
