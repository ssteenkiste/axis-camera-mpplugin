﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AxisCameraMPPlugin.Configuration.Properties
{


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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AxisCameraMPPlugin.Configuration.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Add Camera.
        /// </summary>
        internal static string AddCamera_Title {
            get {
                return ResourceManager.GetString("AddCamera_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to remove the selected camera..
        /// </summary>
        internal static string RemoveCameraWarning {
            get {
                return ResourceManager.GetString("RemoveCameraWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remove Camera.
        /// </summary>
        internal static string RemoveCameraWarning_Title {
            get {
                return ResourceManager.GetString("RemoveCameraWarning_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a network address..
        /// </summary>
        internal static string Validation_Failed_Address {
            get {
                return ResourceManager.GetString("Validation_Failed_Address", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a name of the camera..
        /// </summary>
        internal static string Validation_Failed_CameraName {
            get {
                return ResourceManager.GetString("Validation_Failed_CameraName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a password that consist of one to eight characters. All characters are valid..
        /// </summary>
        internal static string Validation_Failed_Password {
            get {
                return ResourceManager.GetString("Validation_Failed_Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a port in range 1-65535..
        /// </summary>
        internal static string Validation_Failed_Port {
            get {
                return ResourceManager.GetString("Validation_Failed_Port", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a username that contains from one to fourteen characters. Valid characters are: A-Z, a-z, 0-9 and _ (underscore). Username must start with a letter..
        /// </summary>
        internal static string Validation_Failed_Username {
            get {
                return ResourceManager.GetString("Validation_Failed_Username", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The camera is specified by entering network address and HTTP port. Credentials are used to validate you as a trusted user on the camera..
        /// </summary>
        internal static string WizardPageOne_Description {
            get {
                return ResourceManager.GetString("WizardPageOne_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select Camera.
        /// </summary>
        internal static string WizardPageOne_Header {
            get {
                return ResourceManager.GetString("WizardPageOne_Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Finish the operation by specifying a name and snapshot identifying the camera inside MediaPortal..
        /// </summary>
        internal static string WizardPageTwo_Description {
            get {
                return ResourceManager.GetString("WizardPageTwo_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Finish Operation.
        /// </summary>
        internal static string WizardPageTwo_Header {
            get {
                return ResourceManager.GetString("WizardPageTwo_Header", resourceCulture);
            }
        }
    }
}