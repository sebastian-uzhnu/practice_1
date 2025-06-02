namespace Praktika1
{
    static class MainMenu
    {
        public static void Show()
        {
            while (true)
            {
               
                Console.WriteLine("\n========= Меню Реєстарції =========");
                Console.WriteLine("|-------|-------------------------|");
                Console.WriteLine("| 1     | Реєстрація              |");
                Console.WriteLine("| 2     | Авторизація             |");
                Console.WriteLine("| 3     | Вхід як адмін           |");
                Console.WriteLine("| 4     | Вихід                   |");

                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":

                        RegisterUser();
                        break;
                    case "2":
                       
                        if (AuthorizeUser(out string userEmail))
                        {
                            UserMenu.Show(userEmail);
                        }
                        break;
                    case "3":
                        Console.Clear();
                        AdminMenu.Show();
                        break;
                    case "4":
                        Console.WriteLine("Програма завершена.");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private static void RegisterUser()
        {
            Console.WriteLine("\n=== Реєстрація ===");

            Console.Write("Введіть email: ");
            string email = Console.ReadLine();

            if (!ValidateEmail(email))
            {
                Console.WriteLine("Некоректний формат email. Спробуйте ще раз.");
                return;
            }

            Console.Write("Введіть пароль (мінімум 6 символів): ");
            string password = Console.ReadLine();

            if (!ValidatePassword(password))
            {
                Console.WriteLine("Пароль має містити щонайменше 6 символів. Спробуйте ще раз.");
                return;
            }

            string directoryPath = @"data";
            string filePath = Path.Combine(directoryPath, "users.csv");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "Email,Password\n");
            }

            if (File.ReadAllLines(filePath).Any(line => line.StartsWith(email + ",")))
            {
                Console.WriteLine("Користувач із таким email уже існує. Спробуйте ще раз.");
                return;
            }

            File.AppendAllText(filePath, $"{email},{password}\n");
            Console.WriteLine("Реєстрація успішна!");
        }

        private static bool AuthorizeUser(out string userEmail)
        {
            userEmail = null;

            Console.WriteLine("\n=== Авторизація ===");
            Console.Write("Введіть email: ");
            string email = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            string directoryPath = @"data";
            string filePath = Path.Combine(directoryPath, "users.csv");

            if (File.Exists(filePath) && File.ReadAllLines(filePath)
                .Any(line => line == $"{email},{password}"))
            {
                Console.WriteLine("Авторизація успішна!");
                userEmail = email;
                return true;
            }
            else
            {
                Console.WriteLine("Невірний email або пароль. Спробуйте ще раз.");
                return false;
            }
        }

        private static bool ValidateEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private static bool ValidatePassword(string password)
        {
            return password.Length >= 6;
        }
    }
}
