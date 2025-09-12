using Bogus;
using SignNow.Net.Model.Responses;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;
using System.Collections.Generic;
using System.Linq;

namespace SignNow.Net.Test.TestData.FakeModels
{
    /// <summary>
    /// Faker for generating fake GetDocumentGroupTemplatesResponse data
    /// </summary>
    public class GetDocumentGroupTemplatesResponseFaker : Faker<GetDocumentGroupTemplatesResponse>
    {
        public GetDocumentGroupTemplatesResponseFaker()
        {
            RuleFor(x => x.DocumentGroupTemplates, f => new DocumentGroupTemplateFaker().Generate(f.Random.Int(1, 5)));
            RuleFor(x => x.DocumentGroupTemplateTotalCount, f => f.Random.Int(1, 100));
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplate data
    /// </summary>
    public class DocumentGroupTemplateFaker : Faker<DocumentGroupTemplate>
    {
        public DocumentGroupTemplateFaker()
        {
            RuleFor(x => x.FolderId, f => f.Random.Bool() ? f.Random.AlphaNumeric(40) : null);
            RuleFor(x => x.LastUpdated, f => f.Date.Past());
            RuleFor(x => x.TemplateGroupId, f => f.Random.AlphaNumeric(40));
            RuleFor(x => x.TemplateGroupName, f => f.Commerce.ProductName() + " Template Group");
            RuleFor(x => x.OwnerEmail, f => f.Internet.Email());
            RuleFor(x => x.Templates, f => new DocumentGroupTemplateItemFaker().Generate(f.Random.Int(1, 3)));
            RuleFor(x => x.IsPrepared, f => f.Random.Bool());
            RuleFor(x => x.RoutingDetails, f => new DocumentGroupTemplateRoutingDetailsFaker().Generate());
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateItem data
    /// </summary>
    public class DocumentGroupTemplateItemFaker : Faker<DocumentGroupTemplateItem>
    {
        public DocumentGroupTemplateItemFaker()
        {
            RuleFor(x => x.Id, f => f.Random.AlphaNumeric(40));
            RuleFor(x => x.Name, f => f.Commerce.ProductName() + " Template");
            RuleFor(x => x.Thumbnail, f => new ThumbnailFaker().Generate());
            RuleFor(x => x.Roles, f => f.PickRandom(new[] { "Signer", "Approver", "Viewer" }, f.Random.Int(1, 2)).ToList());
        }
    }


    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateRoutingDetails data
    /// </summary>
    public class DocumentGroupTemplateRoutingDetailsFaker : Faker<DocumentGroupTemplateRoutingDetails>
    {
        public DocumentGroupTemplateRoutingDetailsFaker()
        {
            RuleFor(x => x.SignAsMerged, f => f.Random.Bool());
            RuleFor(x => x.IncludeEmailAttachments, f => null);
            RuleFor(x => x.InviteSteps, f => new DocumentGroupTemplateInviteStepFaker().Generate(f.Random.Int(1, 3)));
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateInviteStep data
    /// </summary>
    public class DocumentGroupTemplateInviteStepFaker : Faker<DocumentGroupTemplateInviteStep>
    {
        public DocumentGroupTemplateInviteStepFaker()
        {
            RuleFor(x => x.Order, f => f.Random.Int(1, 10));
            RuleFor(x => x.InviteEmails, f => new DocumentGroupTemplateInviteEmailFaker().Generate(f.Random.Int(1, 2)));
            RuleFor(x => x.InviteActions, f => new DocumentGroupTemplateInviteActionFaker().Generate(f.Random.Int(1, 2)));
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateInviteEmail data
    /// </summary>
    public class DocumentGroupTemplateInviteEmailFaker : Faker<DocumentGroupTemplateInviteEmail>
    {
        public DocumentGroupTemplateInviteEmailFaker()
        {
            RuleFor(x => x.Email, f => f.Internet.Email());
            RuleFor(x => x.Subject, f => f.Lorem.Sentence(3));
            RuleFor(x => x.Message, f => f.Lorem.Paragraph());
            RuleFor(x => x.Reminder, f => new DocumentGroupTemplateReminderFaker().Generate());
            RuleFor(x => x.ExpirationDays, f => f.Random.Int(7, 30));
            RuleFor(x => x.HasSignActions, f => f.Random.Bool());
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateReminder data
    /// </summary>
    public class DocumentGroupTemplateReminderFaker : Faker<DocumentGroupTemplateReminder>
    {
        public DocumentGroupTemplateReminderFaker()
        {
            RuleFor(x => x.RemindBefore, f => f.Random.Int(0, 5));
            RuleFor(x => x.RemindAfter, f => f.Random.Int(0, 5));
            RuleFor(x => x.RemindRepeat, f => f.Random.Int(0, 5));
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateInviteAction data
    /// </summary>
    public class DocumentGroupTemplateInviteActionFaker : Faker<DocumentGroupTemplateInviteAction>
    {
        public DocumentGroupTemplateInviteActionFaker()
        {
            RuleFor(x => x.Email, f => f.Internet.Email());
            RuleFor(x => x.Authentication, f => new DocumentGroupTemplateAuthenticationFaker().Generate());
            RuleFor(x => x.Uuid, f => f.Random.Guid().ToString());
            RuleFor(x => x.AllowReassign, f => f.Random.Int(0, 1));
            RuleFor(x => x.DeclineBySignature, f => f.Random.Int(0, 1));
            RuleFor(x => x.Action, f => f.PickRandom("sign", "approve", "view"));
            RuleFor(x => x.RoleName, f => f.PickRandom("Signer", "Approver", "Viewer"));
            RuleFor(x => x.DocumentId, f => f.Random.AlphaNumeric(40));
            RuleFor(x => x.DocumentName, f => f.Commerce.ProductName() + " Document");
        }
    }

    /// <summary>
    /// Faker for generating fake DocumentGroupTemplateAuthentication data
    /// </summary>
    public class DocumentGroupTemplateAuthenticationFaker : Faker<DocumentGroupTemplateAuthentication>
    {
        public DocumentGroupTemplateAuthenticationFaker()
        {
            RuleFor(x => x.Type, f => f.PickRandom("password", "phone", null));
        }
    }
}
