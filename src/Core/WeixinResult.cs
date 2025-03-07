using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// Represents the result of an Weixin handler operation.
    /// </summary>
    public class WeixinResult
    {
        private static readonly WeixinResult _success = new WeixinResult { Succeeded = true };
        private List<WeixinError> _errors = new List<WeixinError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityError"/>s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="IdentityError"/>s.</value>
        public IEnumerable<WeixinError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="IdentityResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="IdentityResult"/> indicating a successful operation.</returns>
        public static WeixinResult Success => _success;

        /// <summary>
        /// Creates an <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="IdentityError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static WeixinResult Failed(params WeixinError[] errors)
        {
            var result = new WeixinResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="IdentityResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="IdentityResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}
