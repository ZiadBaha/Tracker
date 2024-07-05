using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Models
{
    [NotMapped]
    public class ContentContainer<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }

        public ContentContainer(T data, string message = null)
        {
            Data = data;
            Message = message ?? "Operation successful";
        }
    }
}
