using Console_Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Console_Application.Facade {
    public class Facade {
        private readonly IRepositoryService repositoryService;

        public Facade(ServiceProvider provider) {
            repositoryService = provider.GetService<IRepositoryService>();
        }

        public void ShowMenu() {
            string menu = "What category of operation do you want to preform?\n" +
                "1. Repositories\n" +
                "2. Quit\n";
            Console.Write(menu);
            int option = ReadOption(2);
            ShowSubMenu(option);
        }

        public void ShowSubMenu(int choice) {
            string menu = "unrecognized option";
            
            switch (choice) {
                case 1:
                    menu = "1. View own repositories\n" +
                        "2. Push Changes\n" +
                        "3. Pull Changes\n" +
                        "4. Add Repository\n" +
                        "5. Clone Repository\n"+
                        "6. Revert to earlier version";
                    Console.WriteLine(menu);
                    int c = ReadOption(5);
                    RepositoryHandler(c);
                    break;
                case 2:
                    QuitApplication();
                    break;
            }

        }

        private string AskForString(string message) {
            string res = null;

            do {
                Console.Write(message);
                res = Console.ReadLine();
            } while (string.IsNullOrEmpty(res));

            return res;
        }

        private void QuitApplication() {
            System.Environment.Exit(0);
        }


        public void RepositoryHandler(int choice) {
            switch (choice){
                case 1:
                    repositoryService.GetRepositoriesByCurrentUser().Wait();
                    break;
                case 2:
                    repositoryService.AddChangesAsync().Wait();
                    break;
                case 4:
                    string repoName = AskForString("Enter a name for the repository: ");
                    repositoryService.AddRepository(repoName).Wait();
                    break;
                case 5:
                    repositoryService.CloneRepository().Wait();
                    break;
            }
        }

        private int ReadOption(int max) {
            int choice = 0;

            do {
                try {
                    Console.Write("Your choice:");
                    choice = int.Parse(Console.ReadLine());
                } catch (FormatException fe) {
                    Console.WriteLine("invalid choice");
                    choice = 0;
                }
            } while (choice == 0 || choice > max);

            return choice;
        }
    }
}
