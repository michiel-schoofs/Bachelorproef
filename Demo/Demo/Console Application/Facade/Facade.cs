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
                    menu = "1. View own repositories\n2. View public repositories\n3. View repositories by user";
                    Console.WriteLine(menu);
                    int c = ReadOption(3);
                    RepositoryHandler(c);
                    break;
            }

        }

        public void RepositoryHandler(int choice) {
            switch (choice){
                case 1:
                    repositoryService.GetRepositoriesByCurrentUser().Wait();
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
