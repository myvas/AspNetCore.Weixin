using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinMenuApi
{
    /// <summary>
    /// Publish/create a menu to the Management Platform (MP).
    /// </summary>
    /// <param name="createJson">The Json object by which describes a menu.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeixinErrorJson> PublishMenuAsync(WeixinMenuCreateJson createJson, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publish/create a conditional menu to the Management Platform (MP).
    /// </summary>
    /// <param name="createJson">The Json object by which describes a conditional menu.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeixinMenuPublishConditionalMenuResultJson> PublishConditionalMenuAsync(WeixinConditionalMenuCreateJson createJson, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get current menu of Weixin official account on the Management Platform (MP).
    /// </summary>
    /// <param name="version">
    /// <para>"0" - Try to get the menu created via API calling.</para>
    /// <para>"1" - Try to get the menu managed via the MP system.</para>
    /// <para>"" - Don't care. Just get the menu as it is.</para>
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>The current menu.</returns>
    Task<WeixinCurrentMenuJson> GetCurrentMenuAsync(string version = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the menu with conditional menu (if applicable) on the Management Platform (MP).
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>The menu and conditional menu (if applicable).</returns>
    Task<WeixinConditionalMenuJson> GetMenuAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Test the menu with the specified user, to check the menu whether it is as expected or not.
    /// </summary>
    /// <param name="userId">The OpenId or Weixin account of user.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The menu matching with the specified user</returns>
    Task<WeixinConditionalMenuJson> TryMatchMenuAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the conditional menu specified by a <paramref name="MenuId"/>.
    /// </summary>
    /// <param name="menuId">Id of the menu to be deleted. e.g. "208379533"</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeixinErrorJson> DeleteConditionalMenuAsync(string menuId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the menu on the Management Platform (MP).
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeixinErrorJson> DeleteMenuAsync(CancellationToken cancellationToken = default);
}