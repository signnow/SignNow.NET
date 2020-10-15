using System;
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
        ///   "id": "c376990ca7e1e84ea7f6e252144e435f314bb63b",
        ///   "active": false,
        ///   "verified": true,
        ///   "is_logged_in": false,
        ///   "first_name": "Jesus",
        ///   "last_name": "Monahan",
        ///   "primary_email": "Jesus_Monahan@gmail.com",
        ///   "created": "1580771931",
        ///   "billing_period": {
        ///     "start_date": "09/23/2020",
        ///     "end_date": "10/23/2020",
        ///     "start_timestamp": 1600819200,
        ///     "end_timestamp": 1603411200
        ///   },
        ///   "companies": [
        ///     {
        ///       "name": "Signnow",
        ///       "full_access": true
        ///     }
        ///   ],
        ///   "monthly_document_count": 0,
        ///   "lifetime_document_count": 0
        /// }
        /// </code>
        /// </example>
        public UserFaker()
        {
            Rules((f, o) => {
                o.Id          = f.Random.Hash(40);
                o.Active      = f.Random.Bool();
                o.Verified    = f.Random.Bool();
                o.IsLoggedIn  = f.Random.Bool();
                o.FirstName   = f.Name.FirstName();
                o.LastName    = f.Name.LastName();
                o.Email       = f.Internet.Email(o.FirstName, o.LastName);
                o.Created     = f.Date.Recent().ToUniversalTime();
                o.BillingPeriod = new UserBillingFaker().Generate();
                o.Companies     = new CompanyFaker().Generate(1);
                o.MonthlyDocumentCount = f.Random.Number();
                o.LifetimeDocumentCount = o.MonthlyDocumentCount + f.Random.Number();
            });
        }
    }

    public class UserSignNowFaker : UserFaker
    {
        /// <summary>
        /// Faker <see cref="User"/> with signnow email.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "c376990ca7e1e84ea7f6e252144e435f314bb63b",
        ///   "active": true,
        ///   "verified": true,
        ///   "is_logged_in": false,
        ///   "first_name": "Jesus",
        ///   "last_name": "Monahan",
        ///   "primary_email": "signnow.tutorial+Jesus_Monahan@gmail.com",
        ///   "created": "1580771931",
        ///   "billing_period": {
        ///     "start_date": "09/23/2020",
        ///     "end_date": "10/23/2020",
        ///     "start_timestamp": 1600819200,
        ///     "end_timestamp": 1603411200
        ///   },
        ///   "companies": [
        ///     {
        ///       "name": "Signnow",
        ///       "full_access": true
        ///     }
        ///   ],
        ///   "monthly_document_count": 0,
        ///   "lifetime_document_count": 0
        /// }
        /// </code>
        /// </example>
        public UserSignNowFaker()
        {
            base.FinishWith((f, o) => {
                o.Active      = true;
                o.Verified    = true;
                o.IsLoggedIn  = true;
                o.Email       = "signnow.tutorial+" + f.Internet.Email(o.FirstName, o.LastName, "gmail.com");
                o.Companies     = new CompanyFaker()
                    .FinishWith((f, c) =>
                    {
                        c.Name = "Signnow";
                        c.FullAccess = true;
                    })
                    .Generate(1);
            });
        }
    }

    public class UserBillingFaker : Faker<UserBilling>
    {
        /// <summary>
        /// Faker <see cref="UserBilling"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "start_date": "09/23/2020",
        ///   "end_date": "10/23/2020",
        ///   "start_timestamp": 1600819200,
        ///   "end_timestamp": 1603411200
        /// }
        /// </code>
        /// </example>
        public UserBillingFaker()
        {
            Rules((f, o) =>
            {
                o.StartDate = f.Date.Recent().ToUniversalTime();
                o.EndDate   = f.Date.Recent().Add(TimeSpan.FromDays(14)).ToUniversalTime();
            });
        }
    }

    public class CompanyFaker : Faker<Company>
    {
        /// <summary>
        /// Faker <see cref="Company"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "name": "Signnow",
        ///   "full_access": true
        /// }
        /// </code>
        /// </example>
        public CompanyFaker()
        {
            Rules((f, o) =>
            {
                o.Name = f.Company.CompanyName();
                o.FullAccess = f.Random.Bool();
            });
        }
    }
}
