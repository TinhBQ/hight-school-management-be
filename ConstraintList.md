# **Danh sách các ràng buộc cho thời khóa biểu**

## **1. Các ràng buộc cứng**

### __1.1. Ràng buộc đụng độ phân công (H1)*:__
Trong một thời khóa biểu, các phân công không được xếp vào cùng một vị trí tiết học. Ràng buộc này có nghĩa là: Các phân công của một giáo viên thì không xếp vào cùng 1 vị trí tiết học, các phân công của cùng một lớp thì không xếp vào cùng 1 vị trí tiết học.

### __1.2. Ràng buộc tính đầy đủ của phân công (H2)*:__
Tất cả các phân công phải được xếp vào thời khóa biểu.

### __1.3. Ràng buộc phân công được xếp sẵn (H3)*:__
Các phân công được xếp sẵn (được khóa) vào một vị trí tiết học thì được ưu tiên xếp đầu tiên và các phân công khác cần phải tránh xếp vào vị trí tiết học này.

### **1.4. Ràng buộc đụng độ phòng học (H4):**
Mỗi phòng học chỉ được xếp một phân công trong cùng một thời điểm. Đối với trường THPT Lê Lợi, mỗi một lớp học là tương ứng với một phòng học và sân tập thể dục là sử dụng chung, cho nên thực tế việc đụng độ phòng học chỉ có thể xảy ra ở phòng thực hành tin học.

### **1.5. Ràng buộc một giáo viên không dạy nhiều lớp học cùng lúc (H5):**
Các phân công khác nhau ứng với các lớp học khác nhau của cùng một giáo viên, không được xếp vào cùng một khung giờ học trong cùng một ngày.

### **1.6. Ràng buộc về tiết học liên tiếp (H6):**
Các môn học được chỉ định là có tiết đôi phải có một và chỉ một cặp phân công có khung giờ học liên tiếp trong cùng một ngày.

### **1.7. Ràng buộc về tiết trống của một lớp trong ngày (H7):**
Trong một buổi học, các phân công phải được sắp xếp liên tiếp với nhau, các tiết trống phải được xếp vào cuối buổi.

### **1.8. Ràng buộc về môn học có số lượng giáo viên dạy đồng thời (H8):**
Trong cùng một khung giờ học, số lượng giáo viên dạy cùng một môn học không được lớn hơn số phòng học. Dựa theo mục 1.4, ràng buộc này chỉ áp dụng cho phòng tin học. Điều này có thể hiểu là trong một khung giờ học, số lượng giáo viên dạy môn Tin học không vượt quá số lượng phòng tin học.

### **1.9. Ràng buộc môn học chỉ học 1 lần trong một buổi (H9):**
Trong một buổi học, các phân công được xếp phải là các phân công có môn học khác nhau (trừ cặp phân công được chỉ định là tiết đôi).

### **1.10. Ràng buộc về các tiết cách ngày ():** (đưa xuống ràng buộc mềm )
Trong thời khóa biểu, các phân công có cùng môn học không được xếp vào hai ngày liên tiếp.

### **1.11. Ràng buộc về tiết không xếp (H10):**
Tiết không xếp là một vị trí tiết học được chỉ định cho một giáo viên hoặc một môn học mà phân công của giáo viên hoặc phân công có môn học đó không được xếp vào vị trí tiết học này.

### **1.12. Ràng buộc về ngày không dạy dành cho giáo viên (H11):**
Trong một tuần (trừ chủ nhật), giáo viên phải có ít nhất 1 ngày nghỉ. Tức là ngày đó không có phân công nào được xếp cho giáo viên đó.

## **2. Các ràng buộc mềm**

### **2.1. Ràng buộc về các tiết đôi không có giờ ra chơi chơi xem giữa (S1):**
Cặp phân công được chỉ định là tiết đôi được ưu tiên tránh các cặp tiết học (2, 3) vào buổi sáng và (3, 4) vào buổi chiểu.

### **2.2. Ràng buộc về số lượng buổi dạy của giáo viên (S2):**
Xếp các phân công sao cho số buổi dạy của giáo viên là ít nhất.

### **2.3. Ràng buộc về tiết trống của giáo viên trong trong một buổi học (S3):**
Hạn chế tối đa tiết trống của giáo viên trong một buổi học (nếu được thì có thể tùy chỉnh số lượng tiết trống tối đa trong một buổi học).

### **2.4. Ràng buộc về thời gian nghỉ giữa hai buổi của giáo viên (S4):**
Đối với giáo viên dạy cả hai buổi trong một ngày, hạn chế việc xếp các phân công vào tiết cuối buổi sáng và tiết đầu buổi chiều.

### **2.5. Ràng buộc về số tiết dạy của giáo viên trong một buổi (S5):**
Giới hạn số lượng tiết dạy tối thiểu và tiết dạy tối đa của một giáo viên trong 1 buổi.

### **2.6. Ràng buộc về lịch bận của giáo viên (S6):**
Mỗi giáo viên được chỉ định lịch bận trong tuần, ưu tiên không xếp các phân công của giáo viên vào lịch bận này (ràng buộc này giống với ràng buộc ở mục 1.11, tuy nhiên chương trình sẽ luôn thỏa mãn ràng buộc 1.11, còn ràng buộc này thì không). Có thể tùy chỉnh mức độ ưu tiên cho vị trí tiết học mà giáo viên bận.

## **3. Chú thích**

Một thời khóa biểu ứng với 1 lớp có dạng như sau:

|Tiết/Thứ|Thứ 2|Thứ 3|...|Thứ 7|
|--------|-----|-----|---|-----|
|1|X1|X2|...|...|
|...|...|...|...|...|
|5|...|...|...|Xn|

Trong đó:
- Thứ 2, thứ 3,... được gọi là một ngày.
- Tiết 1, tiết 2,... được gọi là một khung giờ học.
- X1, X2,... được gọi là một vị trí tiết học (hoặc một thời điểm).

*: Các ràng buộc luôn đảm bảo tính đúng đắn khi tạo thời khóa biểu tự động.
