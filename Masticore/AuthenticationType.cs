using System.ComponentModel;

namespace Masticore
{
    /// <summary>
    /// Enumerates the types of authentication supported in the system
    /// </summary>
    public enum AuthenticationType
    {
        [Description("Active Directory")]
        ActiveDirectory,
        [Description("Active Directory B2C")]
        ActiveDirectoryB2C,
        [Description("Okta")]
        Okta
    }
}
