using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Speech.Synthesis;

namespace AoE_Pinger.Classes
{
    class MessageScheduler
    {

        private List<Tuple<string, string>> listToShow;
        private SpeechSynthesizer _synthesizer;

        public MessageScheduler(List<Tuple<string, string>> listToShow)
        {
            this.listToShow = listToShow;
            _synthesizer = new SpeechSynthesizer();
        }

        public async Task ShowMessagesAsync()
        {
            // Create a list to hold all the tasks
            var tasks = new List<Task>();

            foreach (var item in listToShow)
            {
                if (TimeSpan.TryParse(item.Item1, out TimeSpan delay))
                {
                    // Create a task for each item and add it to the list
                    tasks.Add(Task.Delay(delay).ContinueWith(t => ConvertTextToSpeech(item.Item2)));
                }
                else
                {
                    Console.WriteLine($"Invalid time format: {item.Item1}");
                }
            }

            // Await all tasks to complete
            await Task.WhenAll(tasks);
        }


        private void ConvertTextToSpeech(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _synthesizer.SpeakAsync(text);
            }
            else
            {
                MessageBox.Show("Please enter some text to convert to speech.");
            }
        }

    }
}
