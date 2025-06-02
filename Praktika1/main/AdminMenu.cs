namespace Praktika1
{
    static class AdminMenu
    {
        private const string AdminEmail = "admin@gmail.com";
        private const string AdminPassword = "admin123";
        private const string UserFilePath = "data/users.csv";
        private const string AdminFilePath = "data/admin.csv";
        private const string ProfileFilePath = "data/profiles.csv";

        public static void Show()
        {
            Console.WriteLine("\n=== Адміністративне меню ===");

            Console.Write("Введіть email адміністратора: ");
            string email = Console.ReadLine();
            Console.Write("Введіть пароль адміністратора: ");
            string password = Console.ReadLine();

            if (ValidateAdmin(email, password))
            {
                Console.WriteLine("Ви війшли як ADMIN.");
                ShowMenu();
            }
            else
            {
                Console.WriteLine("Невірний email або пароль адміністратора.");
            }
        }

        private static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n======== Адміністративне меню ======");
                Console.WriteLine("|-------|--------------------------|");
                Console.WriteLine("| 1     | Перегляд користувачів    |");
                Console.WriteLine("| 2     | Додати адміністратора    |");
                Console.WriteLine("| 3     | Перегляд анкет           |");
                Console.WriteLine("| 4     | Редагування анкет        |");
                Console.WriteLine("| 5     | Деактивувати користувача |");
                Console.WriteLine("| 6     | Вийти в головне меню     |");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewUsers();
                        break;
                    case "2":
                        AddAdmin();
                        break;
                    case "3":
                        ViewProfiles();
                        break;
                    case "4":
                        EditProfiles();
                        break;
                    case "5":
                        DeactivateUser();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private static void ViewUsers()
        {
            Console.WriteLine("\n=== Список користувачів ===");

            if (!File.Exists(UserFilePath))
            {
                Console.WriteLine("Файл користувачів відсутній.");
                return;
            }

            var users = File.ReadAllLines(UserFilePath).Skip(1).ToList();

            if (users.Count == 0)
            {
                Console.WriteLine("Користувачі відсутні.");
            }
            else
            {
                Console.WriteLine("| N  | Email                     |");
                Console.WriteLine("|----|---------------------------|");
                int index = 1;
                foreach (var user in users)
                {
                    string[] parts = user.Split(',');
                    Console.WriteLine($"| {index,-3}| {parts[0],-25} |");
                    index++;
                }
            }
        }

        private static void AddAdmin()
        {
            Console.WriteLine("\n=== Додати адміністратора ===");
            Console.Write("Введіть email нового адміністратора: ");
            string email = Console.ReadLine();
            Console.Write("Введіть пароль (мінімум 6 символів): ");
            string password = Console.ReadLine();

            if (!File.Exists(AdminFilePath))
            {
                File.WriteAllText(AdminFilePath, "Email,Password\n");
            }
            File.AppendAllText(AdminFilePath, $"{email},{password}\n");
            Console.WriteLine("Адміністратор успішно доданий!");
        }

        private static void ViewProfiles()
        {
            Console.WriteLine("\n=== Перегляд анкет ===");

            if (!File.Exists(ProfileFilePath))
            {
                Console.WriteLine("Файл анкет відсутній.");
                return;
            }

            string[] profiles = File.ReadAllLines(ProfileFilePath);

            if (profiles.Length <= 1)
            {
                Console.WriteLine("Анкети користувачів відсутні.");
                return;
            }

            for (int i = 1; i < profiles.Length; i++)
            {
                Console.WriteLine($"{i}. {profiles[i]}");
            }
        }

        private static void EditProfiles()
        {
            Console.WriteLine("\n=== Редагування анкет ===");

            if (!File.Exists(ProfileFilePath))
            {
                Console.WriteLine("Файл анкет відсутній.");
                return;
            }

            string[] profiles = File.ReadAllLines(ProfileFilePath);

            if (profiles.Length <= 1)
            {
                Console.WriteLine("Анкети користувачів відсутні.");
                return;
            }

            for (int i = 1; i < profiles.Length; i++)
            {
                Console.WriteLine($"{i}. {profiles[i]}");
            }

            Console.Write("\nВведіть номер анкети для редагування або 0 для виходу: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index < profiles.Length)
            {
                string selectedProfile = profiles[index];
                Console.WriteLine($"Вибрана анкета: {selectedProfile}");

                Console.Write("Введіть нове ім'я (або натисніть Enter, щоб залишити без змін): ");
                string newName = Console.ReadLine();

                Console.Write("Введіть новий рік (або натисніть Enter, щоб залишити без змін): ");
                string newYear = Console.ReadLine();

                Console.Write("Введіть новий опис (або натисніть Enter, щоб залишити без змін): ");
                string newDescription = Console.ReadLine();

                string[] parts = selectedProfile.Split(',');
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    parts[0] = newName;
                }
                if (!string.IsNullOrWhiteSpace(newYear))
                {
                    parts[1] = newYear;
                }
                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    parts[2] = newDescription;
                }

                profiles[index] = string.Join(",", parts);
                File.WriteAllLines(ProfileFilePath, profiles);
                Console.WriteLine("Анкета успішно оновлена!");
            }
            else
            {
                Console.WriteLine("Скасовано або введено невірний номер.");
            }
        }

        private static void DeactivateUser()
        {
            Console.WriteLine("\n=== Деактивувати користувача ===");
            Console.Write("Введіть email користувача для деактивації: ");
            string email = Console.ReadLine();

            if (!File.Exists(UserFilePath))
            {
                Console.WriteLine("Файл користувачів відсутній.");
                return;
            }

            string[] users = File.ReadAllLines(UserFilePath);
            var updatedUsers = users.Where(line => !line.StartsWith(email + ",")).ToArray();

            if (users.Length == updatedUsers.Length)
            {
                Console.WriteLine("Користувача з таким email не знайдено.");
            }
            else
            {
                File.WriteAllLines(UserFilePath, updatedUsers);
                Console.WriteLine("Користувача успішно деактивовано.");
            }
        }

        private static bool ValidateAdmin(string email, string password)
        {
            if (email == AdminEmail && password == AdminPassword)
            {
                return true;
            }

            return File.Exists(AdminFilePath) && File.ReadAllLines(AdminFilePath)
                .Any(line => line == $"{email},{password}");
        }
    }
}
