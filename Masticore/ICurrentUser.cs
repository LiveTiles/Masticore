namespace Masticore
{
    /// <summary>
    /// Interface for a user's basic information
    /// </summary>
    public interface IUser
    {
        string ExternalId { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
    }

    /// <summary>
    /// Interface for a user's information at runtime, during requests
    /// </summary>
    public interface ICurrentUser : IUser
    {
        bool IsAuthenticated { get; }
    }

    public interface ICurrentUserWithImage : ICurrentUser
    {
        int Id { get; }
        string ImageUrl { get; }
    }


    /// <summary>
    /// Extensions methods for the ICurrentUser interface
    /// </summary>
    public static class IUserExtensions
    {
        /// <summary>
        /// Gets the full name for the user, this is derived from FirstName and LastName
        /// </summary>
        public static string GetFullName(this IUser user)
        {
            return string.Format("{0} {1}", user.FirstName, user.LastName);
        }
    }
}
