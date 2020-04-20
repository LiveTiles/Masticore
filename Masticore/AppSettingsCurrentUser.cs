using System.Configuration;

namespace Masticore
{
    /// <inheritdoc />
    /// <summary>
    /// Reads the current user settings from the config file - this is a stand-in class for local development
    /// </summary>
    public class AppSettingsCurrentUser : ICurrentUser
    {
        public static readonly string DefaultIdentifier = ConfigurationManager.AppSettings["AppSettingsCurrentUser.DefaultIdentifier"];
        public static readonly string DefaultFirstName = ConfigurationManager.AppSettings["AppSettingsCurrentUser.DefaultFirstName"];
        public static readonly string DefaultLastName = ConfigurationManager.AppSettings["AppSettingsCurrentUser.DefaultLastName"];
        public static readonly string DefaultEmail = ConfigurationManager.AppSettings["AppSettingsCurrentUser.DefaultEmail"];

        /// <summary>
        /// Returns true - always
        /// </summary>
        public bool IsAuthenticated => true;

        /// <summary>
        /// Gets the E-mail specified on the config file
        /// </summary>
        public string Email => DefaultEmail;

        /// <summary>
        /// Gets the identifier specified in the config file
        /// </summary>
        public string ExternalId => DefaultIdentifier;

        /// <summary>
        /// Gets the first name specified in the config file
        /// </summary>
        public string FirstName => DefaultFirstName;

        /// <summary>
        /// Gets the last name specified in the config file
        /// </summary>
        public string LastName => DefaultLastName;
    }
}
