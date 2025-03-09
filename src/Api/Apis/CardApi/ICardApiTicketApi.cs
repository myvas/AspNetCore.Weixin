using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface ICardApiTicketApi
{
    Task<CardApiTicketJson> GetCardApiTicket();
}
