using AoE_Pinger.Classes;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;

namespace AoE_Pinger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Create a new Tuple list to populate later
        public static List<Tuple<string, string>> list_to_show = new List<Tuple<string, string>>();
        public static Boolean hasStarted = false;

        public void LoadDataFromFile(string filePath)
        {
            // Ensure the file exists before trying to read
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            // Clear existing items in the ListView
            list_to_show.Clear();

            // Read all lines from the text file
            string[] lines = File.ReadAllLines(filePath);

            // Loop through each line and process it
            foreach (string line in lines)
            {
                // Split the line using the "||" separator
                string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.None);

                // Check if the split was successful (we expect exactly two parts: time and text)
                if (parts.Length == 2)
                {
                    string timeValue = parts[0];
                    string textValue = parts[1];

                    // Add a new ListViewItem to the ListView
                    list_to_show.Add(new Tuple<string, string>(timeValue, textValue));
                }
            }
        }

        private void Create_new_tasks(object sender, RoutedEventArgs e)
        {
            CustomMessageBox msgBox = new CustomMessageBox("Creator will be available in next versions");
            msgBox.ShowDialog();
        }

        private void Import_saved_tasks(object sender, RoutedEventArgs e)
        {
            CustomFileBrowser fileBrowser = new CustomFileBrowser();
            fileBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(fileBrowser.SelectedFilePath))
            {
                LoadDataFromFile(fileBrowser.SelectedFilePath);
                TaskListView.ItemsSource = list_to_show;
            }
        }

        private static SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of SpeechRecognitionEngine.
            recognizer = new SpeechRecognitionEngine();

            // Create a list of words or phrases that the recognizer can listen for.
            Choices commands = new Choices();
            commands.Add(new string[] { "Let's go", "Go" });

            // Create a Grammar object from the Choices.
            GrammarBuilder gb = new GrammarBuilder(commands);
            Grammar grammar = new Grammar(gb);

            // Load the grammar into the recognizer.
            recognizer.LoadGrammar(grammar);

            // Register an event handler to handle the speech recognized event.
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            // Set the input to the default audio device (microphone).
            recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous speech recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // This is the recognized command.
            string command = e.Result.Text;

            // Stop the recognizer to avoid further recognitions.
            recognizer.RecognizeAsyncStop();

            // Call specific functions based on recognized command.
            if (command == "Let's go")
            {
                WorkingTasks();
            }
            else if (command == "Go")
            {
                WorkingTasks();
            }
        }

        static async Task WorkingTasks()
        {
            if (hasStarted == false)
            {
                hasStarted = true;
                var scheduler = new MessageScheduler(list_to_show);
                await scheduler.ShowMessagesAsync();
            }

        }

        private void button_enabler_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (button_enabler.IsEnabled == true)
            {
                //preparing
                button_enabler.IsEnabled = false;
                WorkingTasks();
            }
        }
    }
}