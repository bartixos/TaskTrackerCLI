using System.Text.Json;

namespace TaskTrackerCLI {
    internal class Program {
        const string fileName = "todo.json";
        static int nextID = 1;
        static void Main(string[] args) {
            if (args.Length == 0) {
                return;
            }
            if (!File.Exists(fileName)) {
                File.Create(fileName).Close();
            }
            string command = args[0].ToLower();


            switch (command) {
                case "help":
                    WriteHelp();
                    break;

                case "add":
                    AddCommand(args[1..]);

                    break;
                case "list":
                    if (args.Length == 1) {
                        WriteTasksConsole(null);
                    }
                    else {
                        WriteTasksConsole(args[1]);
                    }

                    break;
                case "delete":
                    if (args.Length == 2) {
                        int id;
                        if (int.TryParse(args[1], out id)) {
                            DeleteTask(id);
                            break;
                        }
                    }
                    Console.WriteLine("You have to give correct task id");
                    break;
                case "mark-in-progress":
                    if (args.Length == 2) {
                        int id;
                        if (int.TryParse(args[1], out id)) {
                            MarkTask(id, TaskStatus.In_Progres);
                            break;
                        }
                    }

                    Console.WriteLine("To mark in progress task you have to type id of the task");
                    break;
                case "mark-done":
                    if (args.Length == 2) {
                        int id;
                        if (int.TryParse(args[1], out id)) {
                            MarkTask(id, TaskStatus.Done);
                            break;
                        }
                    }
                    break;
                case "update":
                    if (args.Length == 3) {
                        int id;
                        if (int.TryParse(args[1], out id)) {
                            Update(id, args[2]);
                            break;
                        }
                    }

                    Console.WriteLine(@"To update task write it ID then new description
eg. Update 1 ""new task""");
                    break;

            }
            WriteHelp();

        }

        private static void WriteHelp() {
            Console.WriteLine(@"Available commends below
add ""name of a task""          eg. add ""take a dog out"" You can add multiple tasks
update <task id> ""New name""   eg. update 1 ""Make a diner""
delete <task id>                eg. ""delete 1""
mark-in-progress <task id>      eg. ""mark-in-progress 1""
mark-done <task id>             eg. ""mark-done 1""
list                to list all tasks or with parameter
list done           to list all tasks that are done
list todo           to list all tasks that are to do
list in-progress    to list all tasks that are in progress

");
        }
        private static void DeleteTask(int id) {
            List<Task> tasks = ReadAllTasks() ?? [];
            if (tasks.Count == 0) {
                Console.WriteLine("There is no tasks to delete");
                return;

            }

            var removed = tasks.RemoveAll(task => task.ID == id);
            if (removed == 0) {
                Console.WriteLine($"There is no task with ID: {id}");
            }
            else {
                Console.WriteLine($"Task with ID {id} removed");
            }

            SerializeTaskJson(tasks);

        }

        public static void AddCommand(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine(@"To add a task write also a name of the task you want to add
eg.TaskTrackerCLI add ""yourTask""");
                return;
            }

            List<Task> tasks = ReadAllTasks() ?? [];
            nextID = tasks.Count > 0 ? tasks.Max(t => t.ID) + 1 : 1;
            foreach (string name in args) {

                tasks.Add(new Task() {
                    ID = nextID,
                    Description = name,
                    CreatedAt = DateTime.Now,
                    Status = TaskStatus.Todo

                });
                nextID += 1;

            }

            SerializeTaskJson(tasks);

        }

        public static void WriteTasksConsole(string? arg) {
            List<Task>? tasks = ReadAllTasks() ?? [];

            if (tasks is null || tasks.Count == 0) {
                Console.WriteLine("There is no task in a list");
                return;
            }

            if (string.IsNullOrEmpty(arg)) {
                foreach (var task in tasks) {
                    Console.WriteLine(task.ToString());
                }
            }
            else if (arg == "todo") {
                List<Task> result = tasks.FindAll(task => task.Status == TaskStatus.Todo).ToList() ?? [];
                if (result.Count == 0) {
                    Console.WriteLine($"There is no task with status {TaskStatus.Todo.ToString()}");
                    return;
                }
                foreach (var task in result) {
                    Console.WriteLine(task.ToString());
                }

            }
            else if (arg == "done") {
                List<Task> result = tasks.FindAll(task => task.Status == TaskStatus.Done).ToList() ?? [];
                if (result.Count == 0) {
                    Console.WriteLine($"There is no task with status {TaskStatus.Done.ToString()}");
                    return;
                }
                foreach (var task in result) {
                    Console.WriteLine(task.ToString());
                }

            }
            else if (arg == "in-progress") {
                List<Task> result = tasks.FindAll(task => task.Status == TaskStatus.In_Progres).ToList() ?? [];
                if (result.Count == 0) {
                    Console.WriteLine($"There is no task with status {TaskStatus.In_Progres.ToString()}");
                    return;
                }
                foreach (var task in result) {
                    Console.WriteLine(task.ToString());
                }

            }
            else {
                Console.WriteLine("Wrong type list, avaible are only <empty>, todo, done, in-progress");
            }


        }

        public static List<Task>? ReadAllTasks() {
            if (!File.Exists(fileName)) {
                File.Create(fileName).Close();
                return null;
            }
            string jsonSource = File.ReadAllText(fileName);


            if (string.IsNullOrEmpty(jsonSource))
                return null;
            else
                return JsonSerializer.Deserialize<List<Task>>(jsonSource);

        }

        public static void Update(int id, string newName) {

            List<Task> tasks = ReadAllTasks() ?? [];

            if (tasks.Count == 0) {
                Console.WriteLine("There is no task in current list, please first add a task to be able to update");
                return;
            }

            Task? task = tasks.FirstOrDefault(task => task.ID == id);

            if (task is null) {
                Console.WriteLine($"There is no task with ID: {id}");
                return;
            }

            task!.Description = newName;
            task!.UpdatedAt = DateTime.Now;

            SerializeTaskJson(tasks);

            Console.WriteLine($"task updated: {task!.ToString()}");
        }

        public static void SerializeTaskJson(List<Task> tasks) {

            string output = JsonSerializer.Serialize(tasks);
            File.WriteAllText(fileName, output);
        }

        public static void MarkTask(int id, TaskStatus status) {

            List<Task> tasks = ReadAllTasks() ?? [];

            if (tasks.Count == 0) {
                Console.WriteLine("There is no task in current list, please first add a task to be able to update");
                return;
            }
            Task? task = tasks.FirstOrDefault(task => task.ID == id);
            if (task is null) {
                Console.WriteLine($"Could not find task with given ID: {id}");
                return;
            }

            task.Status = status;
            task.UpdatedAt = DateTime.Now;

            SerializeTaskJson(tasks);
            Console.WriteLine($"Task {task.Description} changed status to {task.Status.ToString()}");

        }

    }
}
