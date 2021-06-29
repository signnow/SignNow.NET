﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignNow.Net.Exceptions {
    using System;
    using System.Reflection;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {

        private static System.Resources.ResourceManager resourceMan;

        private static System.Globalization.CultureInfo resourceCulture;

        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("SignNow.Net.Exceptions.ExceptionMessages", typeof(ExceptionMessages).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to RequestUrl cannot be empty or null.
        /// </summary>
        internal static string RequestUrlIsNull {
            get {
                return ResourceManager.GetString("RequestUrlIsNull", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot assign role to user.
        /// </summary>
        internal static string CannotAddRole {
            get {
                return ResourceManager.GetString("CannotAddRole", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Document must have fillable fields.
        /// </summary>
        internal static string NoFillableFieldsWithRole {
            get {
                return ResourceManager.GetString("NoFillableFieldsWithRole", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid format of ID.
        /// </summary>
        internal static string InvalidFormatOfId {
            get {
                return ResourceManager.GetString("InvalidFormatOfId", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid format of email.
        /// </summary>
        internal static string InvalidFormatOfEmail {
            get {
                return ResourceManager.GetString("InvalidFormatOfEmail", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to: Unexpected value when converting to `{0}`. Expected {1}, got {2}.
        /// </summary>
        internal static string UnexpectedValueWhenConverting {
            get {
                return ResourceManager.GetString("UnexpectedValueWhenConverting", resourceCulture);
            }
        }

        /// <summary>
        /// Unable to process request {HttpMethod}:{RequestUrl}, Time taken: {requestTime}s
        /// </summary>
        internal static string UnableToProcessRequest {
            get {
                return ResourceManager.GetString("UnableToProcessRequest", resourceCulture);
            }
        }

        /// <summary>
        /// Invalid Json syntax in response
        /// </summary>
        internal static string InvalidJsonSyntax {
            get {
                return ResourceManager.GetString("InvalidJsonSyntax", resourceCulture);
            }
        }

        /// <summary>
        /// This document does not contain Roles.
        /// </summary>
        internal static string DocumentDoesNotHaveRoles {
            get {
                return ResourceManager.GetString("DocumentDoesNotHaveRoles", resourceCulture);
            }
        }

        /// <summary>
        /// An invite already exists for this document.
        /// </summary>
        internal static string InviteIsAlreadyExistsForDocument {
            get {
                return ResourceManager.GetString("InviteIsAlreadyExistsForDocument", resourceCulture);
            }
        }

        /// <summary>
        /// Allowed range must be from 15 to 45 minutes
        /// </summary>
        internal static string AllowedRangeMustBeFrom15to45 {
            get {
                return ResourceManager.GetString("AllowedRangeMustBeFrom15to45", resourceCulture);
            }
        }

        /// <summary>
        /// String cannot be null, empty or whitespace
        /// </summary>
        internal static string StringNotNullOrEmptyOrWhitespace {
            get {
                return ResourceManager.GetString("StringNotNullOrEmptyOrWhitespace", resourceCulture);
            }
        }
    }
}
