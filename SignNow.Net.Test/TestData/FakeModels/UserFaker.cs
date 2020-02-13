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
            base
                .RuleFor(obj => obj.Active, faker => faker.Random.Bool())
                .RuleFor(obj => obj.FirstName, faker => faker.Name.FirstName())
                .RuleFor(obj => obj.LastName, faker => faker.Name.LastName())
                .RuleFor(
                    obj => obj.Email,
                    (faker, obj) => faker.Internet.Email(obj.FirstName, obj.LastName));
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
            base
                .RuleFor(obj => obj.Active, true)
                .RuleFor(obj => obj.FirstName, faker => faker.Name.FirstName())
                .RuleFor(obj => obj.LastName, faker => faker.Name.LastName())
                .RuleFor(
                    obj => obj.Email,
                    (faker, obj) => "signnow.tutorial+" + faker.Internet.Email(obj.FirstName, obj.LastName, "gmail.com"));
        }
    }
}
