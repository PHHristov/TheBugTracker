using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        [DisplayName("Company")]
        public int? CompanyId { get; set; }

        [Required]
        [DisplayName("Project Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Projcet Description")]
        public string Description { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset EndDate { get; set; }

        [DisplayName("Priority")]
        public int? ProjectPriorityId { get; set; }

        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile ImageFormFile { get; set; }

        public byte[] ImageFileData { get; set; }
        [DisplayName("File Name")]
        public string ImageFileName { get; set; }

        [DisplayName("File Extention")]
        public string ImageContentType { get; set; }

        public bool Archived { get; set; }

        // Navigation Properties
        public virtual Company Company { get; set; }
        public virtual ProjectPriority ProjectPriority { get; set; }

        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
