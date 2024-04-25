namespace Constraints.SoftConstraints
{
    public class SoftConstraint01
    {
        public string Name = "Ràng buộc về các tiết đôi không có giờ ra chơi chơi xem giữa";

        public int Score = 0;

        public string Description = "Cặp phân công được chỉ định là tiết đôi được ưu tiên " +
            "tránh các cặp tiết học (2, 3) vào buổi sáng và (3, 4) vào buổi chiểu.";
    }
}
