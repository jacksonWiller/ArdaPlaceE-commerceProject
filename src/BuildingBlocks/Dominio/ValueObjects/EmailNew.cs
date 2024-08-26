using Ardalis.Result;
using JdMarketplace.Core.Utils;


namespace JdMarketplace.Core.Dominio.ValueObjects;

public sealed record EmailNew
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailNew"/> class.
    /// </summary>
    /// <param name="address">The email address.</param>
    private EmailNew(string address) =>
        Address = address.ToLowerInvariant().Trim();

    /// <summary>
    /// Default constructor for EF/ORM.
    /// </summary>
    public EmailNew() { }

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Creates a new <see cref="EmailNew"/> instance.
    /// </summary>
    /// <param name="emailAddress">The email address to create.</param>
    /// <returns>A <see cref="Result{T}"/> with the created <see cref="EmailNew"/> if successful, or an error message if not.</returns>
    public static Result<EmailNew> Create(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            return Result<EmailNew>.Error("The e-mail address must be provided.");

        return !RegexPatterns.EmailIsValid.IsMatch(emailAddress)
            ? Result<EmailNew>.Error("The e-mail address is invalid.")
            : Result<EmailNew>.Success(new EmailNew(emailAddress));
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="EmailNew"/> object.
    /// </summary>
    /// <returns>A string that represents the current <see cref="EmailNew"/> object.</returns>
    public override string ToString() => Address;
}