﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Myvas.AspNetCore.Weixin {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Myvas.AspNetCore.Weixin.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Failed on WeChat message signature verification!.
        /// </summary>
        internal static string InvalidSignatureDenied {
            get {
                return ResourceManager.GetString("InvalidSignatureDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please access this page via the WeChat client (Official name: Micromessenger)!.
        /// </summary>
        internal static string NonMicroMessengerDenied {
            get {
                return ResourceManager.GetString("NonMicroMessengerDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to AddEntityFrameworkStores can only be called with a Weixin subscriber that derives from WeixinSubscriber..
        /// </summary>
        internal static string NotWeixinSubscriber {
            get {
                return ResourceManager.GetString("NotWeixinSubscriber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are now trying to visit a verification URL for a WeChat Official Account server.  You can fill this URL in the “Development/Basic Configuration/Server Configuration/Server Address (URL)” field in the WeChat Official Account backend (https://mp.weixin.qq.com)..
        /// </summary>
        internal static string WeixinSiteVerificationPathNotice {
            get {
                return ResourceManager.GetString("WeixinSiteVerificationPathNotice", resourceCulture);
            }
        }
    }
}
