using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TemplateGenerator.Utilities;

namespace TemplateGenerator
{
    public partial class MainWindow : Window
    {
        private readonly List<int> _defaultIDs;
        public MainWindow()
        {
            InitializeComponent();

            // Check local folders for any existing IDs, then set the initial ranges based on them
            _defaultIDs = new FolderScanner(@"Images\client_1\").DefaultIDs;
            InputBoxes.SetDefaultRanges(_defaultIDs, Client1_Input0, Client1_Input1);
            _defaultIDs = new FolderScanner(@"Images\client_2\").DefaultIDs;
            InputBoxes.SetDefaultRanges(_defaultIDs, Client2_Input0, Client2_Input1);
            _defaultIDs = new FolderScanner(@"Images\client_3\").DefaultIDs;
            InputBoxes.SetDefaultRanges(_defaultIDs, Client3_Input0, Client3_Input1);
            _defaultIDs = new FolderScanner(@"Images\client_4\").DefaultIDs;
            InputBoxes.SetDefaultRanges(_defaultIDs, Client4_Input0, Client4_Input1);
            _defaultIDs = new FolderScanner(@"Images\client_5\").DefaultIDs;
            InputBoxes.SetDefaultRanges(_defaultIDs, Client5_Input0, Client5_Input1);

            LoadSavedUserInputs();
        }

        // Make sure the user can only introduce numbers
        private void InputNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = MyRegex();
            e.Handled = regex.IsMatch(e.Text);
        }

        // Generate random, unique IDs within the ranges provided by the user
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ClientProfile clientProfile = new();
            TextBox[] inputTextBoxes =
                { Client1_Input0, Client1_Input1, Client2_Input0, Client2_Input1, Client3_Input0, Client3_Input1, Client4_Input0, Client4_Input1 };

            // Check if the user didn't leave any bracket empty
            if (inputTextBoxes.Any(box => string.IsNullOrWhiteSpace(box.Text)))
            {
                MessageBox.Show("Make sure that all the empty brackets have a number.");
                return;
            }

            // If the user inputs are valid, implement a profile for each final client
            
            // Client1
            if (!InputBoxes.IsValidRange(Client1_Input0, Client1_Input1, 3)) return;
            ClientProfile.GenerateClientProfile(Client1_Input0, Client1_Input1, Client1_Result0, Client1_Result1, Client1_Result2);

            // Client2
            if (!InputBoxes.IsValidRange(Client2_Input0, Client2_Input1, 5)) return;
            ClientProfile.GenerateClientProfile(Client2_Input0, Client2_Input1, Client2_Result0, Client2_Result1, Client2_Result2, Client2_Result3, Client2_Result4);

            // Client3
            if (!InputBoxes.IsValidRange(Client3_Input0, Client3_Input1, 3)) return;
            ClientProfile.GenerateClientProfile(Client3_Input0, Client3_Input1, Client3_Result0, Client3_Result1, Client3_Result2);

            // Client4
            if (!InputBoxes.IsValidRange(Client4_Input0, Client4_Input1, 4)) return;
            ClientProfile.GenerateClientProfile(Client4_Input0, Client4_Input1, Client4_Result0, Client4_Result1, Client4_Result2, Client4_Result3);

            // Client5
            if (!InputBoxes.IsValidRange(Client5_Input0, Client5_Input1, 3)) return;
            ClientProfile.GenerateClientProfile(Client5_Input0, Client5_Input1, Client5_Result0, Client5_Result1, Client5_Result2);

            // Save the inputs to the local machine
            int[] userInputs = new int[] {
                int.Parse(Client1_Input0.Text),
                int.Parse(Client1_Input1.Text),
                int.Parse(Client2_Input0.Text),
                int.Parse(Client2_Input1.Text),
                int.Parse(Client3_Input0.Text),
                int.Parse(Client3_Input1.Text),
                int.Parse(Client4_Input0.Text),
                int.Parse(Client4_Input1.Text),
                int.Parse(Client5_Input0.Text),
                int.Parse(Client5_Input1.Text),
            };
            SaveUserInputs(userInputs);
        }

        // Store the user inputs on the local machine
        private static void SaveUserInputs(int[] inputs)
        {
            using IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            using IsolatedStorageFileStream isoStream = new("userinputs.txt", FileMode.Create, isoStore);
            using StreamWriter writer = new(isoStream);
            try
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    writer.WriteLine(inputs[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Load user inputs saved during the previous use of the application 
        private void LoadSavedUserInputs()
        {
            using IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            if (isoStore.FileExists("userinputs.txt"))
            {
                using IsolatedStorageFileStream isoStream = new("userinputs.txt", FileMode.Open, isoStore);
                using StreamReader reader = new(isoStream);
                int[] inputs = new int[10];
                for (int i = 0; i < 10; i++)
                {
                    string? inputStr = reader.ReadLine();
                    inputs[i] = string.IsNullOrEmpty(inputStr) ? 0 : int.Parse(inputStr);
                }
                Client1_Input0.Text = inputs[0].ToString();
                Client1_Input1.Text = inputs[1].ToString();
                Client2_Input0.Text = inputs[2].ToString();
                Client2_Input1.Text = inputs[3].ToString();
                Client3_Input0.Text = inputs[4].ToString();
                Client3_Input1.Text = inputs[5].ToString();
                Client4_Input0.Text = inputs[6].ToString();
                Client4_Input1.Text = inputs[7].ToString();
                Client5_Input0.Text = inputs[8].ToString();
                Client5_Input1.Text = inputs[9].ToString();
            }
        }

        // Allow the user to move the application's window around on screen 
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var window = sender as Window;
                window?.DragMove();
            }
        }

        // Allow the user to exit the application 
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // On click redirect from the logo to the company's portal  
        private void LogoRedirect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.diversitate-incluziune.com/") 
                { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while trying to open the website: " + ex.Message);
            }
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();

        internal void Run(MainWindow mainWindow)
        {
            throw new NotImplementedException();
        }
    }
}
