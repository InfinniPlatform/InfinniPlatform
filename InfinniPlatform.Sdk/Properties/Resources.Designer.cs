﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InfinniPlatform.Sdk.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InfinniPlatform.Sdk.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fail to get document with exception: {0}.
        /// </summary>
        internal static string FailGetDocument {
            get {
                return ResourceManager.GetString("FailGetDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to query result is not of array type.
        /// </summary>
        internal static string ResultIsNotOfArrayType {
            get {
                return ResourceManager.GetString("ResultIsNotOfArrayType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to query result is not of object type.
        /// </summary>
        internal static string ResultIsNotOfObjectType {
            get {
                return ResourceManager.GetString("ResultIsNotOfObjectType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Service not registered! Please check type of request (GET,POST) and name of invoked service instance..
        /// </summary>
        internal static string ServiceNotRegistered {
            get {
                return ResourceManager.GetString("ServiceNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client session not initialized..
        /// </summary>
        internal static string SessionNotInitialized {
            get {
                return ResourceManager.GetString("SessionNotInitialized", resourceCulture);
            }
        }
    }
}
