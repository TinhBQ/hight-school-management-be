namespace Constraints.HardConstraints
{
    public class HardConstraint06
    {
        public const string Name = "Ràng buộc về các môn có tiết đôi";

        public const int Score = -10000;

        public const string Description = "Các môn học được chỉ định là có tiết đôi phải có một " +
            "và chỉ một cặp phân công có khung giờ học liên tiếp trong cùng một ngày.";
    }
}
