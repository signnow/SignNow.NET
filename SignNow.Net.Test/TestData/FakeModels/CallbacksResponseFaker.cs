using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for generating <see cref="CallbacksResponse"/> test data.
    /// Provides various factory methods for different callback scenarios.
    /// </summary>
    public class CallbacksResponseFaker : Faker<CallbacksResponse>
    {
        private readonly CallbackFaker _callbackFaker;
        private readonly CallbackMetaInfoFaker _metaInfoFaker;

        /// <summary>
        /// Initializes a new instance of <see cref="CallbacksResponseFaker"/>.
        /// </summary>
        /// <example>
        /// This example shows Json representation of a CallbacksResponse.
        /// <code>
        /// {
        ///   "data": [
        ///     {
        ///       "id": "callback_123456789",
        ///       "application_name": "MyApp",
        ///       "entity_id": "doc_987654321",
        ///       "event_subscription_id": "sub_123456",
        ///       "event_subscription_active": true,
        ///       "entity_type": "document",
        ///       "event_name": "document.complete",
        ///       "callback_url": "https://example.com/webhook",
        ///       "request_method": "POST",
        ///       "duration": 1.5,
        ///       "request_start_time": 1609459200,
        ///       "request_end_time": 1609459205,
        ///       "request_headers": {
        ///         "string_head": "header_value",
        ///         "int_head": 42,
        ///         "bool_head": true,
        ///         "float_head": 3.14
        ///       },
        ///       "response_content": "OK",
        ///       "response_status_code": 200,
        ///       "event_subscription_owner_email": "owner@example.com",
        ///       "request_content": { ... }
        ///     }
        ///   ],
        ///   "meta": {
        ///     "pagination": {
        ///       "total": 1,
        ///       "count": 1,
        ///       "per_page": 50,
        ///       "current_page": 1,
        ///       "total_pages": 1,
        ///       "links": {
        ///         "previous": null,
        ///         "next": null
        ///       }
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public CallbacksResponseFaker()
        {
            _callbackFaker = new CallbackFaker();
            _metaInfoFaker = new CallbackMetaInfoFaker();

            RuleFor(cr => cr.Data, f => _callbackFaker.Generate(f.Random.Int(1, 5)).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.Generate());
        }

        /// <summary>
        /// Generates a response with successful callbacks (2xx status codes).
        /// </summary>
        /// <param name="count">Number of successful callbacks to generate.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for successful callbacks.</returns>
        public CallbacksResponseFaker WithSuccessfulCallbacks(int count = 3)
        {
            RuleFor(cr => cr.Data, f => _callbackFaker.Successful().Generate(count).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(count, count).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with failed callbacks (4xx/5xx status codes).
        /// </summary>
        /// <param name="count">Number of failed callbacks to generate.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for failed callbacks.</returns>
        public CallbacksResponseFaker WithFailedCallbacks(int count = 2)
        {
            RuleFor(cr => cr.Data, f => _callbackFaker.Failed().Generate(count).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(count, count).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with document event callbacks only.
        /// </summary>
        /// <param name="count">Number of document event callbacks to generate.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for document events.</returns>
        public CallbacksResponseFaker WithDocumentEventCallbacks(int count = 3)
        {
            RuleFor(cr => cr.Data, f => _callbackFaker.DocumentEvent().Generate(count).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(count, count).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with user event callbacks only.
        /// </summary>
        /// <param name="count">Number of user event callbacks to generate.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for user events.</returns>
        public CallbacksResponseFaker WithUserEventCallbacks(int count = 3)
        {
            RuleFor(cr => cr.Data, f => _callbackFaker.UserEvent().Generate(count).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(count, count).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with template event callbacks only.
        /// </summary>
        /// <param name="count">Number of template event callbacks to generate.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for template events.</returns>
        public CallbacksResponseFaker WithTemplateEventCallbacks(int count = 2)
        {
            RuleFor(cr => cr.Data, f => _callbackFaker.TemplateEvent().Generate(count).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(count, count).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with group event callbacks only.
        /// </summary>
        /// <param name="count">Number of group event callbacks to generate.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for group events.</returns>
        public CallbacksResponseFaker WithGroupEventCallbacks(int count = 2)
        {
            RuleFor(cr => cr.Data, f => _callbackFaker.DocumentGroupEvent().Generate(count).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(count, count).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with mixed event types.
        /// </summary>
        /// <param name="documentCount">Number of document event callbacks.</param>
        /// <param name="userCount">Number of user event callbacks.</param>
        /// <param name="templateCount">Number of template event callbacks.</param>
        /// <param name="groupCount">Number of group event callbacks.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured with mixed event types.</returns>
        public CallbacksResponseFaker WithMixedEvents(int documentCount = 2, int userCount = 2, int templateCount = 1, int groupCount = 1)
        {
            var totalCount = documentCount + userCount + templateCount + groupCount;
            
            RuleFor(cr => cr.Data, f =>
            {
                var callbacks = new List<Callback<CallbackContentAllFields>>();
                
                if (documentCount > 0)
                    callbacks.AddRange(_callbackFaker.DocumentEvent().Generate(documentCount));
                    
                if (userCount > 0)
                    callbacks.AddRange(_callbackFaker.UserEvent().Generate(userCount));
                    
                if (templateCount > 0)
                    callbacks.AddRange(_callbackFaker.TemplateEvent().Generate(templateCount));
                    
                if (groupCount > 0)
                    callbacks.AddRange(_callbackFaker.DocumentGroupEvent().Generate(groupCount));

                // Shuffle the list to simulate mixed ordering
                return f.Random.Shuffle(callbacks).ToList().AsReadOnly();
            })
            .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(totalCount, totalCount).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with mixed status codes (success and failures).
        /// </summary>
        /// <param name="successCount">Number of successful callbacks.</param>
        /// <param name="failureCount">Number of failed callbacks.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured with mixed status codes.</returns>
        public CallbacksResponseFaker WithMixedStatusCodes(int successCount = 3, int failureCount = 2)
        {
            var totalCount = successCount + failureCount;
            
            RuleFor(cr => cr.Data, f =>
            {
                var callbacks = new List<Callback<CallbackContentAllFields>>();
                
                if (successCount > 0)
                    callbacks.AddRange(_callbackFaker.Successful().Generate(successCount));
                    
                if (failureCount > 0)
                    callbacks.AddRange(_callbackFaker.Failed().Generate(failureCount));

                // Shuffle the list to simulate mixed ordering
                return f.Random.Shuffle(callbacks).ToList().AsReadOnly();
            })
            .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(totalCount, totalCount).Generate());

            return this;
        }

        /// <summary>
        /// Generates an empty response with no callbacks.
        /// </summary>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured for empty response.</returns>
        public CallbacksResponseFaker WithEmptyResponse()
        {
            RuleFor(cr => cr.Data, f => new List<Callback<CallbackContentAllFields>>().AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithTotals(0, 0).Generate());

            return this;
        }

        /// <summary>
        /// Generates a response with custom pagination settings.
        /// </summary>
        /// <param name="currentPage">Current page number.</param>
        /// <param name="perPage">Items per page.</param>
        /// <param name="totalItems">Total number of items.</param>
        /// <param name="callbacksToGenerate">Number of callbacks to generate for this page.</param>
        /// <returns>A <see cref="CallbacksResponseFaker"/> configured with custom pagination.</returns>
        public CallbacksResponseFaker WithPagination(int currentPage, int perPage, int totalItems, int callbacksToGenerate = 0)
        {
            if (callbacksToGenerate == 0)
            {
                callbacksToGenerate = Math.Min(perPage, Math.Max(0, totalItems - ((currentPage - 1) * perPage)));
            }

            RuleFor(cr => cr.Data, f => _callbackFaker.Generate(callbacksToGenerate).AsReadOnly())
                .RuleFor(cr => cr.Meta, f => _metaInfoFaker.WithPagination(currentPage, perPage, totalItems, callbacksToGenerate).Generate());

            return this;
        }
    }

    /// <summary>
    /// Faker for generating <see cref="MetaInfo"/> test data.
    /// </summary>
    internal class CallbackMetaInfoFaker : Faker<MetaInfo>
    {
        private readonly CallbackPaginationFaker _paginationFaker;

        public CallbackMetaInfoFaker()
        {
            _paginationFaker = new CallbackPaginationFaker();
            
            RuleFor(mi => mi.Pagination, f => _paginationFaker.Generate());
        }

        public CallbackMetaInfoFaker WithTotals(int total, int count)
        {
            RuleFor(mi => mi.Pagination, f => _paginationFaker.WithTotals(total, count).Generate());
            return this;
        }

        public CallbackMetaInfoFaker WithPagination(int currentPage, int perPage, int total, int count)
        {
            RuleFor(mi => mi.Pagination, f => _paginationFaker.WithPagination(currentPage, perPage, total, count).Generate());
            return this;
        }
    }

    /// <summary>
    /// Faker for generating <see cref="Pagination"/> test data.
    /// </summary>
    internal class CallbackPaginationFaker : Faker<Pagination>
    {
        public CallbackPaginationFaker()
        {
            RuleFor(p => p.Total, f => f.Random.Int(1, 100))
                .RuleFor(p => p.Count, (f, p) => Math.Min(p.Total, f.Random.Int(1, 50)))
                .RuleFor(p => p.PerPage, f => f.Random.Int(10, 50))
                .RuleFor(p => p.CurrentPage, f => f.Random.Int(1, 5))
                .RuleFor(p => p.TotalPages, (f, p) => (int)Math.Ceiling((double)p.Total / p.PerPage))
                .RuleFor(p => p.Links, f => new CallbackPageLinksFaker().Generate());
        }

        public CallbackPaginationFaker WithTotals(int total, int count)
        {
            RuleFor(p => p.Total, f => total)
                .RuleFor(p => p.Count, f => count)
                .RuleFor(p => p.PerPage, f => f.Random.Int(Math.Max(count, 10), 50))
                .RuleFor(p => p.CurrentPage, f => 1)
                .RuleFor(p => p.TotalPages, (f, p) => Math.Max(1, (int)Math.Ceiling((double)p.Total / p.PerPage)));

            return this;
        }

        public CallbackPaginationFaker WithPagination(int currentPage, int perPage, int total, int count)
        {
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)total / perPage));

            RuleFor(p => p.Total, f => total)
                .RuleFor(p => p.Count, f => count)
                .RuleFor(p => p.PerPage, f => perPage)
                .RuleFor(p => p.CurrentPage, f => currentPage)
                .RuleFor(p => p.TotalPages, f => totalPages)
                .RuleFor(p => p.Links, f => new CallbackPageLinksFaker().WithPagination(currentPage, totalPages).Generate()
                );

            return this;
        }
    }

    /// <summary>
    /// Faker for generating <see cref="PageLinks"/> test data.
    /// </summary>
    internal class CallbackPageLinksFaker : Faker<PageLinks>
    {
        public CallbackPageLinksFaker()
        {
            RuleFor(pl => pl.Previous, f => f.Random.Bool() ? new Uri(f.Internet.Url()) : null)
                .RuleFor(pl => pl.Next, f => f.Random.Bool() ? new Uri(f.Internet.Url()) : null);
        }

        public CallbackPageLinksFaker WithPagination(int currentPage, int totalPages)
        {
            RuleFor(pl => pl.Previous, f => currentPage > 1 ? 
                new Uri($"https://api.signnow.com/v2/callback?page={currentPage - 1}") : null)
                .RuleFor(pl => pl.Next, f => currentPage < totalPages ? 
                    new Uri($"https://api.signnow.com/v2/callback?page={currentPage + 1}") : null);

            return this;
        }
    }
}
