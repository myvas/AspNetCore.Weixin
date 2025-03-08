using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IMediaApi
{
    Task<BatchGetMaterialsJson> BatchGetMaterials(string materialType, int offset, int count, CancellationToken cancellationToken = default);
    Task<string> DownloadFile(string mediaId, string filePath = "", string folderName = "MediaFiles", string fileName = "", string fileExtensionName = "", CancellationToken cancellationToken = default);
    Task<BatchGetMaterialsJson> GetAllMaterialsAsync(MaterialType materialType, CancellationToken cancellationToken = default);
    Task<MaterialCountJson> GetMaterialCount(CancellationToken cancellationToken = default);
    Task<MediaUploadResultJson> Upload(UploadMediaType type, string file, CancellationToken cancellationToken = default);
    Task<UploadMediaResult> UploadNews(CancellationToken cancellationToken = default, params NewsModel[] news);
}
