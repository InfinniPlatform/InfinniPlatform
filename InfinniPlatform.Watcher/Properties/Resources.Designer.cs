﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InfinniPlatform.Watcher.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InfinniPlatform.Watcher.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to File is busy by another process. Trying again ({0}/{1})..
        /// </summary>
        internal static string BusyFileCopyAttempt {
            get {
                return ResourceManager.GetString("BusyFileCopyAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cant&apos;t copy file..
        /// </summary>
        internal static string CantCopyFile {
            get {
                return ResourceManager.GetString("CantCopyFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Changes within directory{0}{1}{0}will be transferred to directory{0}{2}..
        /// </summary>
        internal static string ChangesWillBeTransferred {
            get {
                return ResourceManager.GetString("ChangesWillBeTransferred", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DestinationDirectory in watcher settings cannot be empty..
        /// </summary>
        internal static string DestinationDirectoryCannotBeEmpty {
            get {
                return ResourceManager.GetString("DestinationDirectoryCannotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}{1}{0}File {2} was {3}..
        /// </summary>
        internal static string EventLog {
            get {
                return ResourceManager.GetString("EventLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to InfinniPlatform.Watcher failed to start..
        /// </summary>
        internal static string FailedStart {
            get {
                return ResourceManager.GetString("FailedStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add watcher setting to AppExtentions.json configuration file:
        ///&quot;watcher&quot;: {
        ///    /* Matadata source directory */
        ///    &quot;SourceDirectory&quot;: &quot;&lt;path&gt;&quot;,
        ///    /* Directory to synchronize */
        ///    &quot;DestinationDirectory&quot;: &quot;&lt;path&gt;&quot;,
        ///    /* File extentions to synchronize */
        ///    &quot;WatchingFileExtensions&quot;: [&quot;json&quot;, &quot;js&quot;, &quot;css&quot;, &quot;html&quot;]
        ///}.
        /// </summary>
        internal static string SettingsExample {
            get {
                return ResourceManager.GetString("SettingsExample", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SourceDictionary in watcher settings cannot be empty..
        /// </summary>
        internal static string SourceDictionaryCannotBeEmpty {
            get {
                return ResourceManager.GetString("SourceDictionaryCannotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to InfinniPlatform.Watcher is active..
        /// </summary>
        internal static string SuccessStart {
            get {
                return ResourceManager.GetString("SuccessStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sync complete..
        /// </summary>
        internal static string SyncComplete {
            get {
                return ResourceManager.GetString("SyncComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Content of directories{2}{0}{2}and{2}{1}{2}are different..
        /// </summary>
        internal static string SyncingContentDirectories {
            get {
                return ResourceManager.GetString("SyncingContentDirectories", resourceCulture);
            }
        }
    }
}
