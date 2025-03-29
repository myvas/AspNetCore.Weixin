using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinMediaApi
{
    Task<WeixinBatchGetMaterialsJson> BatchGetMaterials(string materialType, int offset, int count, CancellationToken cancellationToken = default);
    Task<string> DownloadFile(string mediaId, string filePath = "", string folderName = "MediaFiles", string fileName = "", string fileExtensionName = "", CancellationToken cancellationToken = default);
    Task<WeixinBatchGetMaterialsJson> GetAllMaterialsAsync(WeixinMaterialType materialType, CancellationToken cancellationToken = default);
    Task<WeixinMaterialCountJson> GetMaterialCount(CancellationToken cancellationToken = default);
    Task<WeixinMediaUploadResultJson> Upload(WeixinUploadMediaType type, string file, CancellationToken cancellationToken = default);
    Task<WeixinUploadMediaResult> UploadNews(CancellationToken cancellationToken = default, params WeixinNewsModel[] news);
}
