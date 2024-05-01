namespace Constraints.HardConstraints
{
    public class HardConstraint11
    {
        public const string Name = "Ràng buộc về ngày không dạy dành cho giáo viên";

        public const int Score = -10000;

        public const string Description = "Trong một tuần (trừ chủ nhật), " +
            "giáo viên phải có ít nhất 1 ngày nghỉ. " +
            "Tức là ngày đó không có phân công nào được xếp cho giáo viên đó.";
    }
}
