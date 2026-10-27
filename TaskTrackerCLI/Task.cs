namespace TaskTrackerCLI {
    public enum TaskStatus {
        Todo,
        In_Progres,
        Done,



    }
    public class Task {
        public int ID { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString() {

            string dateToString = CreatedAt.ToString("yyyy-MM-dd H:mm:ss");
            string? updatedDate = UpdatedAt == null ? "" : "Updated: " + UpdatedAt!?.ToString("yyyy-MM-dd H:mm:ss");
            return $"ID: {ID}, Description: {Description}, Status: {Status.ToString()}, Created at: {dateToString}, {updatedDate}";
        }
    }
}
