namespace Constraints.HardConstraints
{
    public class HardConstraint05
    {
        public const string Name = "Ràng buộc một giáo viên không dạy nhiều lớp học cùng lúc";

        public const int Score = -10000;

        public const string Description = "Các phân công khác nhau ứng với các lớp học khác nhau " +
            "của cùng một giáo viên, không được xếp vào cùng một khung giờ học trong cùng một ngày.";
    }
}
