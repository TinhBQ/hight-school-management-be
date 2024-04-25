namespace Constraints.HardConstraints
{
    public class HardConstraint01
    {
        public const string Name = "Ràng buộc đụng độ phân công";

        public const int Score = -10000;

        public const string Description = "Trong một thời khóa biểu, " +
            "các phân công không được xếp vào cùng một vị trí tiết học. " +
            "Ràng buộc này có nghĩa là: Các phân công của một giáo viên " +
            "thì không xếp vào cùng 1 vị trí tiết học, các phân công của " +
            "cùng một lớp thì không xếp vào cùng 1 vị trí tiết học.";
    }
}
