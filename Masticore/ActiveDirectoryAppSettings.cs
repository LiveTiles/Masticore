using System.Configuration;

namespace Masticore
{
    /// <summary>
    /// Reads the config file for the standard-named AD app settings
    /// This is useful for implementing your own strategy or using Masticore.ActiveDirectory for an out-of-the-box strategy
    /// This is included in Masticore since Masticore.Infrastructure needs it, but we don't want to include all the MVC stuff from Masticore.ActiveDirectory
    /// </summary>
    public static class ActiveDirectoryAppSettings
    {
        /// <summary>
        /// Gets the Client ID, the unique app identifier in AD
        /// ida:ClientId
        /// </summary>
        public static readonly string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        /// <summary>
        /// Gets the Client Secret, the unique app key
        /// ida:ClientSecret
        /// </summary>
        public static readonly string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        /// <summary>
        /// Gets the Domain for AD, the URL for the STS server that is shared for all apps (generally microsoft URL)
        /// ida:AADInstance
        /// </summary>
        public static readonly string AADInstance = ConfigurationManager.AppSettings["ida:AADInstance"];

        /// <summary>
        /// Gets the unique domain for the AD tenant, usually [something].onmicrosoft.com
        /// ida:Domain
        /// </summary>
        public static readonly string Domain = ConfigurationManager.AppSettings["ida:Domain"];

        /// <summary>
        /// Gets the unique ID for the AD tenant, usually a GUID-style string
        /// ida:TenantId
        /// </summary>
        public static readonly string TenantId = ConfigurationManager.AppSettings["ida:TenantId"];

        /// <summary>
        /// Gets the name for the AD tenant, which is the domain prefix.
        /// ida:TenantName
        /// </summary>
        public static readonly string TenantName = ConfigurationManager.AppSettings["ida:TenantName"];


        /// <summary>
        /// Gets the target URL (generally pointing back to this app) for after the user logs out of the system
        /// ida:PostLogoutRedirectUri
        /// </summary>
        public static readonly string PostLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        /// <summary>
        /// Gets the redirect URL when authenticating with B2C active directory
        /// ida:RedirectUri
        /// </summary>
        public static readonly string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        /// <summary>
        /// Gets the name of the SignIn policy for this app
        /// ida:SignInPolicyId
        /// </summary>
        public static readonly string SignInPolicyId = ConfigurationManager.AppSettings["ida:SignInPolicyId"];

        /// <summary>
        /// Gets the name of the SignUp policy for this app  
        /// ida:SignUpPolicyId
        /// </summary>
        public static readonly string SignUpPolicyId = ConfigurationManager.AppSettings["ida:SignUpPolicyId"];

        /// <summary>
        /// Gets the name of the user profile policy for this app
        /// ida:UserProfilePolicyId
        /// </summary>
        public static readonly string UserProfilePolicyId = ConfigurationManager.AppSettings["ida:UserProfilePolicyId"];

        static bool? _IsOrgAuthentication = null;

        /// <summary>
        /// Gets a flag indicating if the current mode is org (true) or B2C (false)
        /// This will default to true if the config file does not have a value in it
        /// ida:ActiveDirectoryIsOrg
        /// </summary>
        public static bool IsOrgAuthentication
        {
            get
            {
                if (!_IsOrgAuthentication.HasValue)
                {
                    var isOrg = true;
                    bool.TryParse(ConfigurationManager.AppSettings["ida:ActiveDirectoryIsOrg"], out isOrg);
                    _IsOrgAuthentication = isOrg;
                }

                return _IsOrgAuthentication.Value;
            }
        }
    }
}
