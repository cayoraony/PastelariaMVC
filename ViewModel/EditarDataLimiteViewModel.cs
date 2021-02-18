using System;
using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.ViewModel
{
    public class EditarDataLimiteViewModel
    {
        [Required]
        public DateTime DataLimite { get; set; }

    }
}