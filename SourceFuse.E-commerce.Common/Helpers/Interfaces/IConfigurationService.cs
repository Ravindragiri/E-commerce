using SourceFuse.E_commerce.Common.Enums;

namespace SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces
{
    public interface IConfigurationService
    {
        string GetAdminUserName();
        string GetAdminEmail();
        string GetAdminRoleName();
        string GetAdminPassword();
        int GetDefaultPage();
        int GetDefaultPageSize();
        string GetManageProductPolicyName();
        string GetCreateCommentPolicyName();
        string GetDeleteCommentPolicyName();
        string GetWhoIsAllowedToManageProducts();
        string GetWhoIsAllowedToCreateComments();
        AuthorizationPolicyEnum GetWhoIsAllowedToDeleteComments();
        AuthorizationPolicyEnum GetWhoIsAllowedToUpdateComments();
        string GetStandardUserRoleName();
        string GetAdminFirstName();
        string GetAdminLastName();

        string GetUpdateCommentPolicyName();
    }
}
