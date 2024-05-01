namespace Constraints.HardConstraints
{
    public class HardConstraint03
    {
        public const string Name = "Ràng buộc phân công được xếp sẵn";

        public const int Score = -10000;

        public const string Description = "Các phân công được xếp sẵn (được khóa)" +
            " vào một vị trí tiết học thì được ưu tiên xếp đầu tiên " +
            "và các phân công khác cần phải tránh xếp vào vị trí tiết học này.";
    }
}
