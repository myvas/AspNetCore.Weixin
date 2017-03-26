using System;
using System.Security.Claims;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinAccessTicket
    {
        /// <summary>
        /// Gets the claims-principal with authenticated user identities.
        /// </summary>
        public ClaimsPrincipal Principal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeixinAccessTicket"/> class
        /// </summary>
        /// <param name="principal">the <see cref="ClaimsPrincipal"/> that represents the authenticated user.</param>
        public WeixinAccessTicket(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
        }
    }
}