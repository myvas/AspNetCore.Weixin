using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IQrcodeApi
{
    Task<CreateQrCodeResult> Create(int expireSeconds, int sceneId);
    Task<CreateQrCodeResult> Create(string actionName, string sceneStr, CancellationToken cancellationToken = default);
    string ShowQrcode(string ticket);
    Task ShowQrcode(string ticket, Stream stream);
}
