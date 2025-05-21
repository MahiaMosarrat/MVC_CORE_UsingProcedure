namespace PatientManagementCore.Models.ViewModels
{
    public class AggregateViewModel
    {
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal SumValue { get; set; }
        public decimal AvgValue { get; set; }
        public ICollection<GroupByViewModel> GroupByResult { get; set; }
    }
    public class GroupByViewModel
    {
        public int SpecialistId { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal SumValue { get; set; }
        public decimal AvgValue { get; set; }
    }
}
