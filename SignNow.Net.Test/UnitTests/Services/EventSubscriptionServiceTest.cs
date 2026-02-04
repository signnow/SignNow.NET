using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for EventSubscriptionService.GetCallbacksAsync method.
    /// </summary>
    [TestClass]
    public class EventSubscriptionServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task GetCallbacksAsync_WithValidResponse_ShouldReturnCallbacks()
        {
            var mockResponse = TestUtils.SerializeToJsonFormatted(new
            {
                data = new[]
                {
                    new
                    {
                        id = "callback_123",
                        application_name = "TestApp",
                        entity_id = "doc_456",
                        event_subscription_id = "sub_789",
                        event_subscription_active = true,
                        entity_type = "document",
                        event_name = "document.complete",
                        callback_url = "https://example.com/webhook",
                        request_method = "POST",
                        duration = 1.5,
                        request_start_time = 1609459200,
                        request_end_time = 1609459205,
                        request_headers = new
                        {
                            string_head = "test_value",
                            int_head = 42,
                            bool_head = true,
                            float_head = 3.14f
                        },
                        response_content = "OK",
                        response_status_code = 200,
                        event_subscription_owner_email = "owner@example.com",
                        request_content = new
                        {
                            meta = new
                            {
                                timestamp = 1609459200,
                                @event = "document.complete",
                                environment = "https://api.signnow.com/",
                                initiator_id = "user_789",
                                callback_url = "https://example.com/webhook",
                                access_token = "***masked***"
                            },
                            content = new
                            {
                                document_id = "doc_456",
                                document_name = "Test Document.pdf",
                                user_id = "user_789",
                                template_id = (string)null,
                                invite_id = "invite_123",
                                signer = "signer@example.com",
                                status = "completed",
                                old_invite_unique_id = (string)null,
                                group_id = (string)null,
                                group_name = (string)null,
                                group_invite = (string)null,
                                group_invite_id = (string)null,
                                initiator_id = "user_789",
                                initiator_email = "initiator@example.com",
                                viewer_user_unique_id = (string)null
                            }
                        }
                    }
                },
                meta = new
                {
                    pagination = new
                    {
                        total = 1,
                        count = 1,
                        per_page = 50,
                        current_page = 1,
                        total_pages = 1
                    }
                }
            });

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var options = new GetCallbacksOptions
            {
                Page = 1,
                PerPage = 50
            };

            var response = await service.GetCallbacksAsync(options).ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.AreEqual(1, response.Data.Count);
            var callback = response.Data[0];
            Assert.AreEqual("callback_123", callback.Id);
            Assert.AreEqual("TestApp", callback.ApplicationName);
            Assert.AreEqual("doc_456", callback.EntityId);
            Assert.AreEqual("sub_789", callback.EventSubscriptionId);
            Assert.IsTrue(callback.EventSubscriptionActive);
            Assert.AreEqual(EventSubscriptionEntityType.Document, callback.EntityType);
            Assert.AreEqual(EventType.DocumentComplete, callback.EventName);
            Assert.AreEqual("https://example.com/webhook", callback.CallbackUrl.ToString());
            Assert.AreEqual("POST", callback.RequestMethod);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 1500), callback.Duration);
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1609459200).DateTime, callback.RequestStartTime);
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1609459205).DateTime, callback.RequestEndTime);
            Assert.AreEqual("OK", callback.ResponseContent);
            Assert.AreEqual(200, callback.ResponseStatusCode);
            Assert.AreEqual("owner@example.com", callback.EventSubscriptionOwnerEmail);

            // Test request headers
            Assert.IsNotNull(callback.RequestHeaders);
            Assert.AreEqual("test_value", callback.RequestHeaders.StringHead);
            Assert.AreEqual(42, callback.RequestHeaders.IntHead);
            Assert.IsTrue(callback.RequestHeaders.BoolHead);
            Assert.AreEqual(3.14f, callback.RequestHeaders.FloatHead, 0.01f);

            // Test request content
            Assert.IsNotNull(callback.RequestContent);
            Assert.IsNotNull(callback.RequestContent.Meta);
            Assert.IsNotNull(callback.RequestContent.Content);

            var meta = callback.RequestContent.Meta;
            Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(1609459200).DateTime, meta.Timestamp);
            Assert.AreEqual(EventType.DocumentComplete, meta.Event);
            Assert.AreEqual("https://api.signnow.com/", meta.Environment.ToString());
            Assert.AreEqual("user_789", meta.InitiatorId);
            Assert.AreEqual("https://example.com/webhook", meta.CallbackUrl.ToString());
            Assert.AreEqual("***masked***", meta.AccessToken);

            var content = callback.RequestContent.Content;
            Assert.AreEqual("doc_456", content.DocumentId);
            Assert.AreEqual("Test Document.pdf", content.DocumentName);
            Assert.AreEqual("user_789", content.UserId);
            Assert.IsNull(content.TemplateId);
            Assert.AreEqual("invite_123", content.InviteId);
            Assert.AreEqual("signer@example.com", content.Signer);
            Assert.AreEqual("completed", content.Status);
            Assert.IsNull(content.OldInviteUniqueId);
            Assert.IsNull(content.GroupId);
            Assert.IsNull(content.GroupName);
            Assert.IsNull(content.GroupInvite);
            Assert.IsNull(content.GroupInviteId);
            Assert.AreEqual("user_789", content.InitiatorId);
            Assert.AreEqual("initiator@example.com", content.InitiatorEmail);
            Assert.IsNull(content.ViewerUserUniqueId);

            // Test pagination
            Assert.AreEqual(1, response.Meta.Pagination.Total);
            Assert.AreEqual(1, response.Meta.Pagination.Count);
            Assert.AreEqual(50, response.Meta.Pagination.PerPage);
            Assert.AreEqual(1, response.Meta.Pagination.CurrentPage);
            Assert.AreEqual(1, response.Meta.Pagination.TotalPages);
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithNoOptions_ShouldReturnEmptyResponse()
        {
            var mockResponse = TestUtils.SerializeToJsonFormatted(new
            {
                data = new object[0],
                meta = new
                {
                    pagination = new
                    {
                        total = 0,
                        count = 0,
                        per_page = 50,
                        current_page = 1,
                        total_pages = 0
                    }
                }
            });

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = await service.GetCallbacksAsync().ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.AreEqual(0, response.Data.Count);
            Assert.AreEqual(0, response.Meta.Pagination.Total);
            Assert.AreEqual(0, response.Meta.Pagination.Count);
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithFilterOptions_ShouldBuildCorrectQuery()
        {
            var fakeResponse = new CallbacksResponseFaker().WithSuccessfulCallbacks(3);
            var mockResponse = TestUtils.SerializeToJsonFormatted(fakeResponse.Generate());

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var options = new GetCallbacksOptions
            {
                Filters = f => f.And(
                    fb => fb.EntityId.Like("doc_%"),
                    fb => fb.Code.Between(200, 299)
                ),
                Sortings = s => s.StartTime(SortOrder.Descending).Code(SortOrder.Ascending),
                Page = 2,
                PerPage = 25
            };

            var response = await service.GetCallbacksAsync(options).ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(3, response.Data.Count);
            Assert.IsTrue(response.Data.All(c => c.ResponseStatusCode >= 200 && c.ResponseStatusCode < 300));
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithMultipleCallbackTypes_ShouldReturnMixedContent()
        {
            var fakeResponse = new CallbacksResponseFaker().WithMixedEvents(2, 2, 1, 1);
            var mockResponse = TestUtils.SerializeToJsonFormatted(fakeResponse.Generate());

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = await service.GetCallbacksAsync().ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(6, response.Data.Count);

            // Verify we have different entity types
            var entityTypes = response.Data.Select(c => c.EntityType).Distinct().ToList();
            Assert.IsTrue(entityTypes.Count > 1, "Should have multiple entity types");

            foreach (var callback in response.Data)
            {
                Assert.IsNotNull(callback.RequestContent);
                Assert.IsNotNull(callback.RequestContent.Content);

                var content = callback.RequestContent.Content;
                // At minimum, content should have some basic fields populated
                Assert.IsTrue(!string.IsNullOrEmpty(content.DocumentId) ||
                              !string.IsNullOrEmpty(content.UserId) ||
                              !string.IsNullOrEmpty(content.GroupId),
                              "Content should have at least one identifying field populated");
            }
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithFailedCallbacks_ShouldReturnErrorStatusCodes()
        {
            var fakeResponse = new CallbacksResponseFaker().WithFailedCallbacks(2);
            var mockResponse = TestUtils.SerializeToJsonFormatted(fakeResponse.Generate());

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = await service.GetCallbacksAsync().ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Data.Count);

            foreach (var callback in response.Data)
            {
                Assert.IsTrue(callback.ResponseStatusCode >= 400,
                    $"Expected error status code (>=400), but got {callback.ResponseStatusCode}");
            }
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithDocumentEvents_ShouldReturnOnlyDocumentCallbacks()
        {
            var fakeResponse = new CallbacksResponseFaker().WithDocumentEventCallbacks(3);
            var mockResponse = TestUtils.SerializeToJsonFormatted(fakeResponse.Generate());

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = await service.GetCallbacksAsync().ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Data.Count);

            foreach (var callback in response.Data)
            {
                Assert.AreEqual(EventSubscriptionEntityType.Document, callback.EntityType);
                Assert.IsTrue(callback.EntityId.StartsWith("doc_"),
                    $"Expected document entity ID to start with 'doc_', but got '{callback.EntityId}'");
            }
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithPaginationOptions_ShouldRespectPagination()
        {
            var fakeResponse = new CallbacksResponseFaker();
            var generatedResponse = fakeResponse.Generate();

            generatedResponse.Meta.Pagination.CurrentPage = 2;
            generatedResponse.Meta.Pagination.PerPage = 10;
            generatedResponse.Meta.Pagination.Total = 25;
            generatedResponse.Meta.Pagination.TotalPages = 3;

            var mockResponse = TestUtils.SerializeToJsonFormatted(generatedResponse);

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var options = new GetCallbacksOptions
            {
                Page = 2,
                PerPage = 10
            };

            var response = await service.GetCallbacksAsync(options).ConfigureAwait(false);

            Assert.IsNotNull(response.Meta.Pagination);
            Assert.AreEqual(2, response.Meta.Pagination.CurrentPage);
            Assert.AreEqual(10, response.Meta.Pagination.PerPage);
            Assert.AreEqual(25, response.Meta.Pagination.Total);
            Assert.AreEqual(3, response.Meta.Pagination.TotalPages);
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithNullRequestHeaders_ShouldHandleGracefully()
        {
            var mockResponse = TestUtils.SerializeToJsonFormatted(
                new {
                    data = new[] { new {
                        id = "callback_null_headers",
                        application_name = "TestApp",
                        entity_id = "doc_123",
                        event_subscription_id = "sub_456",
                        event_subscription_active = true,
                        entity_type = "document",
                        event_name = "document.update",
                        callback_url = "https://example.com/webhook",
                        request_method = "POST",
                        duration = 0.5,
                        request_start_time = 1609459200,
                        request_end_time = 1609459201,
                        request_headers = (object)null,
                        response_content = "Success",
                        response_status_code = 200,
                        event_subscription_owner_email = "owner@example.com",
                        request_content = new {
                            meta = new {
                                timestamp = 1609459200,
                                @event = "document.update",
                                environment = "https://api.signnow.com",
                                initiator_id = "user_123",
                                callback_url = "https://example.com/webhook",
                                access_token = "***masked***"
                            },
                            content = new {
                                document_id = "doc_123",
                                document_name = "Updated Document.pdf",
                                user_id = "user_123"
                            }
                        }
                    }},
                    meta = new {
                        pagination = new {
                            total = 1,
                            count = 1,
                            per_page = 50,
                            current_page = 1,
                            total_pages = 1
                        }
                    }
                });
            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = await service.GetCallbacksAsync().ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count);
            var callback = response.Data[0];
            Assert.IsNull(callback.RequestHeaders);
            Assert.AreEqual("callback_null_headers", callback.Id);
            Assert.IsNotNull(callback.RequestContent);
            Assert.IsNotNull(callback.RequestContent.Content);
        }

        [TestMethod]
        public async Task GetCallbacksBySubscriptionIdAsync_WithValidSubscriptionId_ShouldReturnCallbacks()
        {
            var subscriptionId = "8a49e32e267e42a18e4a3967669f347e06e3b71e";
            var mockResponse = TestUtils.SerializeToJsonFormatted(new
            {
                data = new[]
                {
                    new
                    {
                        id = "callback_123",
                        application_name = "TestApp",
                        entity_id = "doc_456",
                        event_subscription_id = subscriptionId,
                        event_subscription_active = true,
                        entity_type = "document",
                        event_name = "document.complete",
                        callback_url = "https://example.com/webhook",
                        request_method = "POST",
                        duration = 1.5,
                        request_start_time = 1609459200,
                        request_end_time = 1609459205,
                        request_headers = new
                        {
                            string_head = "test_value",
                            int_head = 42,
                            bool_head = true,
                            float_head = 3.14f
                        },
                        response_content = "OK",
                        response_status_code = 200,
                        event_subscription_owner_email = "owner@example.com",
                        request_content = new
                        {
                            meta = new
                            {
                                timestamp = 1609459200,
                                @event = "document.complete",
                                environment = "https://api.signnow.com/",
                                initiator_id = "user_789",
                                callback_url = "https://example.com/webhook",
                                access_token = "***masked***"
                            },
                            content = new
                            {
                                document_id = "doc_456",
                                document_name = "Test Document.pdf",
                                user_id = "user_789",
                                initiator_id = "user_789",
                                initiator_email = "initiator@example.com"
                            }
                        }
                    }
                },
                meta = new
                {
                    pagination = new
                    {
                        total = 1,
                        count = 1,
                        per_page = 50,
                        current_page = 1,
                        total_pages = 1
                    }
                }
            });

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var options = new GetCallbacksOptions
            {
                Page = 1,
                PerPage = 50
            };

            var response = await service.GetCallbacksBySubscriptionIdAsync(subscriptionId, options).ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.AreEqual(1, response.Data.Count);
            
            var callback = response.Data[0];
            Assert.AreEqual("callback_123", callback.Id);
            Assert.AreEqual("TestApp", callback.ApplicationName);
            Assert.AreEqual("doc_456", callback.EntityId);
            Assert.AreEqual(subscriptionId, callback.EventSubscriptionId);
            Assert.IsTrue(callback.EventSubscriptionActive);
            Assert.AreEqual(EventSubscriptionEntityType.Document, callback.EntityType);
            Assert.AreEqual(EventType.DocumentComplete, callback.EventName);
            Assert.AreEqual("https://example.com/webhook", callback.CallbackUrl.ToString());
            Assert.AreEqual(200, callback.ResponseStatusCode);

            Assert.AreEqual(1, response.Meta.Pagination.Total);
            Assert.AreEqual(1, response.Meta.Pagination.Count);
            Assert.AreEqual(50, response.Meta.Pagination.PerPage);
        }
    }
}
