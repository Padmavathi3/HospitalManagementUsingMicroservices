using System.ComponentModel.DataAnnotations;

namespace PatientManagementService.Entities
{
    public class PatientEntity
    {
        [Required]
        public string PatientID { get; set; }
        [Required]
        public string MedicalHistory { get; set; }
        [Required]
        public string Insurance { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }
    }
}
