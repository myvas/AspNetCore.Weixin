namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Storage;

/// <summary>
/// If an entity class implements this interface, its modified time will be automatically saved.
/// </summary>
public interface IModifiedTime
{
    /// <summary>
    /// The modified time.
    /// </summary>
    public DateTime? ModifiedTime { get; set; }
}
