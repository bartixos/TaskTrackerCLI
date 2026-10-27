namespace Tests {
    public class TaskTest {
        [Fact]
        public void NewTask_HasCorrectProperties() {
            var task = new TaskTrackerCLI.Task {
                ID = 1,
                Description = "Learn c#",
                Status = TaskTrackerCLI.TaskStatus.Todo,
                CreatedAt = DateTime.Now,
            };

            string result = task.ToString();

            Assert.Contains("ID: 1", result);
            Assert.Contains("Description: Learn c#", result);
            Assert.Contains("Status: Todo", result);
        }
        [Fact]
        public void NewTask_UpdatedAtIsNull() {
            var
        }
    }
}
