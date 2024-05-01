namespace Constraints.HardConstraints
{
    public class HardConstraint08
    {
        public const string Name = "Ràng buộc về môn học có số lượng giáo viên dạy đồng thời";

        public const int Score = -10000;

        public const string Description = "Trong cùng một khung giờ học, số lượng giáo viên dạy " +
            "cùng một môn học không được lớn hơn số phòng học.";
    }
}
