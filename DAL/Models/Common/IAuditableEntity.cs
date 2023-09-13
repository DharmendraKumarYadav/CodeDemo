using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Common
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
