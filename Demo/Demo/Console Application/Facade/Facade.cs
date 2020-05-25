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
            string menu = "What category of operation do you want to preform?\n1. Repositories\n";
            Console.Write(menu);
            int option = ReadOption(1);
            ShowSubMenu(option);
        }

        public void ShowSubMenu(int choice) {
            string menu = "unrecognized option";
            
            switch (choice) {
                case 1:
                    menu = "1. View own repositories\n" +
                        "2. View public repositories\n" +
                        "3. View repositories by user\n" +
                        "4. Add Repository";
                    Console.WriteLine(menu);
                    int c = ReadOption(4);
                    RepositoryHandler(c);
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

        public void RepositoryHandler(int choice) {
            switch (choice){
                case 1:
                    repositoryService.GetRepositoriesByCurrentUser().Wait();
                    break;
                case 4:
                    string repoName = AskForString("Enter a name for the repository: ");
                    repositoryService.AddRepository(repoName).Wait();
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
