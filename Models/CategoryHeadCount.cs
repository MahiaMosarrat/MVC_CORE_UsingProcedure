namespace PatientManagementCore.Models
{
    public class CategoryHeadCount
    {
        public int SpecialistId { get; set; }
        public int Count { get; set; }
        public virtual Specialist Specialist { get; set; }
        public string CategoryName { get; set; }
    }
}
