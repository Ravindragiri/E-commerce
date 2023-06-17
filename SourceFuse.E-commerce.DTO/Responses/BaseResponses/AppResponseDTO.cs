using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.BaseResponses
{
    public class AppResponseDTO
    {
        public AppResponseDTO(bool success)
        {
            Success = success;
        }

        protected AppResponseDTO(bool success, string message) : this(success)
        {
            if (FullMessages == null)
                FullMessages = new List<string>();
            FullMessages.Add(message);
        }

        public bool Success { get; set; }
        public ICollection<string> FullMessages { get; set; } = new List<string>();
    }
}
