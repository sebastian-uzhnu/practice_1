namespace Praktika1
{
    static class UserMenu
    {
        private static string currentUserEmail;

        public static void Show(string email)
        {
            currentUserEmail = email;

            while (true)
            {
                Console.WriteLine("\n======= Меню користувача ===========");
                Console.WriteLine("|-------|--------------------------|");
                Console.WriteLine("| 1     | Створити анкету          |");
                Console.WriteLine("| 2     | Переглянути свої анкети  |");
                Console.WriteLine("| 3     | Редагувати свої анкети   |");
                Console.WriteLine("| 4     | Видалити свою анкету     |");
                Console.WriteLine("| 5     | Вийти в головне меню     |");

                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateProfile();
                        break;
                    case "2":
                        ViewProfiles();
                        break;
                    case "3":
                        EditProfile();
                        break;
                    case "4":
                        DeleteProfile();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private static void CreateProfile()
        {
            Console.WriteLine("\n=== Створення анкети ===");

            Console.Write("Введіть ім'я: ");
            string name = Console.ReadLine();

            Console.Write("Введіть вік: ");
            string age = Console.ReadLine();

            Console.Write("Введіть опис про себе: ");
            string description = Console.ReadLine();

            string directoryPath = @"data";
            string filePath = Path.Combine(directoryPath, "profiles.csv");

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "Email,Name,Age,Description\n");
            }

            File.AppendAllText(filePath, $"{currentUserEmail},{name},{age},{description}\n");
            Console.WriteLine("Анкета успішно створена!");
        }

        private static void ViewProfiles()
        {
            Console.WriteLine("\n=== Ваші анкети ===");

            string directoryPath = @"data";
            string filePath = Path.Combine(directoryPath, "profiles.csv");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Анкети відсутні.");
                return;
            }

            var profiles = File.ReadAllLines(filePath)
                .Where(line => line.StartsWith($"{currentUserEmail},"))
                .ToList();

            if (profiles.Count == 0)
            {
                Console.WriteLine("У вас немає створених анкет.");
            }
            else
            {
                Console.WriteLine("| N | Ім'я                | Вік  | Опис                  |");
                Console.WriteLine("|---|---------------------|------|-----------------------|");
                int index = 1;
                foreach (var profile in profiles)
                {
                    string[] parts = profile.Split(',');
                    Console.WriteLine($"| {index,-2}| {parts[1],-19} | {parts[2],-4} | {parts[3],-22}|");
                    index++;
                }
            }
        }

        private static void EditProfile()
        {
            Console.WriteLine("\n=== Редагування анкети ===");

            string directoryPath = @"data";
            string filePath = Path.Combine(directoryPath, "profiles.csv");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл з анкетами відсутній.");
                return;
            }

            var profiles = File.ReadAllLines(filePath).ToList();
            var userProfiles = profiles.Where(line => line.StartsWith($"{currentUserEmail},")).ToList();

            if (userProfiles.Count == 0)
            {
                Console.WriteLine("У вас немає створених анкет.");
                return;
            }

            for (int i = 0; i < userProfiles.Count; i++)
            {
                string[] parts = userProfiles[i].Split(',');
                Console.WriteLine($"{i + 1}. Ім'я: {parts[1]}, Вік: {parts[2]}, Опис: {parts[3]}");
            }

            Console.Write("\nВведіть номер анкети для редагування: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= userProfiles.Count)
            {
                string selectedProfile = userProfiles[index - 1];
                Console.WriteLine($"Вибрана анкета: {selectedProfile}");

                Console.Write("Введіть нове ім'я (або натисніть Enter, щоб залишити без змін): ");
                string newName = Console.ReadLine();

                Console.Write("Введіть новий вік (або натисніть Enter, щоб залишити без змін): ");
                string newAge = Console.ReadLine();

                Console.Write("Введіть новий опис (або натисніть Enter, щоб залишити без змін): ");
                string newDescription = Console.ReadLine();

                string[] parts = selectedProfile.Split(',');
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    parts[1] = newName;
                }

                if (!string.IsNullOrWhiteSpace(newAge))
                {
                    parts[2] = newAge;
                }

                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    parts[3] = newDescription;
                }

                userProfiles[index - 1] = string.Join(",", parts);

                profiles = profiles.Where(line => !line.StartsWith($"{currentUserEmail},")).ToList();
                profiles.AddRange(userProfiles);
                File.WriteAllLines(filePath, profiles);

                Console.WriteLine("Анкета успішно оновлена!");
            }
            else
            {
                Console.WriteLine("Скасовано або введено невірний номер.");
            }
        }

        private static void DeleteProfile()
        {
            Console.WriteLine("\n=== Видалення анкети ===");

            string directoryPath = @"data";
            string filePath = Path.Combine(directoryPath, "profiles.csv");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл з анкетами відсутній.");
                return;
            }

            var profiles = File.ReadAllLines(filePath).ToList();
            var userProfiles = profiles.Where(line => line.StartsWith($"{currentUserEmail},")).ToList();

            if (userProfiles.Count == 0)
            {
                Console.WriteLine("У вас немає створених анкет.");
                return;
            }

            for (int i = 0; i < userProfiles.Count; i++)
            {
                string[] parts = userProfiles[i].Split(',');
                Console.WriteLine($"{i + 1}. Ім'я: {parts[1]}, Вік: {parts[2]}, Опис: {parts[3]}");
            }

            Console.Write("\nВведіть номер анкети для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= userProfiles.Count)
            {
                userProfiles.RemoveAt(index - 1);

                profiles = profiles.Where(line => !line.StartsWith($"{currentUserEmail},")).ToList();
                profiles.AddRange(userProfiles);
                File.WriteAllLines(filePath, profiles);

                Console.WriteLine("Анкета успішно видалена!");
            }
            else
            {
                Console.WriteLine("Скасовано або введено невірний номер.");
            }
        }
    }
}
