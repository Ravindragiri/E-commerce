using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.DTO.Responses.BaseResponses;

namespace SourceFuse.E_commerce.DTO.Responses.Wrappers
{
    public class StatusCodeResponseDTOWrapper : ObjectResult
    {
        public StatusCodeResponseDTOWrapper(AppResponseDTO dto, int statusCode = 200) : base(dto)
        {
            StatusCode = statusCode;
        }

        private StatusCodeResponseDTOWrapper(AppResponseDTO dto, int statusCode, string message) : base(dto)
        {
            StatusCode = statusCode;
            if (dto.FullMessages == null)
                dto.FullMessages = new List<string>(1);
            dto.FullMessages.Add(message);
        }

        private StatusCodeResponseDTOWrapper(AppResponseDTO dto, int statusCode, ICollection<string> messages) : base(dto)
        {
            StatusCode = statusCode;
            dto.FullMessages = messages;
        }

        public static IActionResult BuildGenericNotFound()
        {
            return new StatusCodeResponseDTOWrapper(new ErrorResponseDTO("Not Found"), 404);
        }

        public static StatusCodeResponseDTOWrapper BuilBadRequest(ModelStateDictionary modelStateDictionary)
        {
            ErrorResponseDTO errorRes = new ErrorResponseDTO();

            foreach (var key in modelStateDictionary.Keys)
            {
                foreach (var error in modelStateDictionary[key].Errors)
                {
                    errorRes.FullMessages.Add(error.ErrorMessage);
                }
            }

            return new StatusCodeResponseDTOWrapper(errorRes, 400);
        }

        public static IActionResult BuildSuccess(AppResponseDTO dto)
        {
            return new StatusCodeResponseDTOWrapper(dto, 200);
        }

        public static IActionResult BuildSuccess(AppResponseDTO dto, string message)
        {
            return new StatusCodeResponseDTOWrapper(dto, 200, message);
        }

        public static IActionResult BuildSuccess(string message)
        {
            return new StatusCodeResponseDTOWrapper(new SuccessResponseDTO(message), 200);
        }

        public static IActionResult BuildErrorResponse(string message)
        {
            return new StatusCodeResponseDTOWrapper(new ErrorResponseDTO(message), 500);
        }

        public static IActionResult BuildGeneric(AppResponseDTO dto, ICollection<string> messages = null,
            int statusCode = 200)
        {
            return new StatusCodeResponseDTOWrapper(dto, statusCode, messages);
        }

        public static IActionResult BuildBadRequest(IEnumerable<IdentityError> resultErrors)
        {
            ErrorResponseDTO res = new ErrorResponseDTO();
            foreach (var resultError in resultErrors)
                res.FullMessages.Add(resultError.Description);

            return new StatusCodeResponseDTOWrapper(res, 400);
        }

        public static IActionResult BuildUnauthorized(ICollection<string> errors = null)
        {
            ErrorResponseDTO res = new ErrorResponseDTO();
            if (errors != null)
            {
                foreach (var error in errors)
                    res.FullMessages.Add(error);
            }

            return new StatusCodeResponseDTOWrapper(res, 401);
        }

        public static IActionResult BuildUnauthorized(string message = null)
        {
            if (message != null)
            {
                List<string> fullMessages = new List<string>(1);
                fullMessages.Add(message);
                return BuildUnauthorized(fullMessages);
            }

            return BuildUnauthorized((ICollection<string>)null);
        }

        public static IActionResult BuildNotFound(AppResponseDTO responseDto)
        {
            return new StatusCodeResponseDTOWrapper(responseDto, 404);
        }
    }
}
