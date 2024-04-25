namespace Constraints.HardConstraints
{
    public class HardConstraint09
    {
        public const string Name = "Ràng buộc môn học chỉ học 1 lần trong một buổi";

        public const int Score = -10000;

        public const string Description = "Trong một buổi học, các phân công được xếp phải " +
            "là các phân công có môn học khác nhau (trừ cặp phân công được chỉ định là tiết đôi).";
    }
}
