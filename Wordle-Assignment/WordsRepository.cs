﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wordle_Assignment
{
    // Cian Dicker-Hughes
    // G00415413@atu.ie

    public class WordsRepository
    {
        // Variables
        private bool isBusy;
        private Random random;
        private List<string> wordslist;
        HttpClient httpClient;

        public WordsRepository()
        {
            wordslist = new List<string>();
            random = new Random();
            httpClient = new();
            
        }

        // to see if busy
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsNotBusy => !IsBusy;

        // get word from git and make text file
        public async Task GetWords()
        {
            if (wordslist.Count > 0)
            {
                return;
            }
            string filename = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "words.txt");
            if (!File.Exists(filename)){
                var response = await httpClient.GetAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");

                if (response.IsSuccessStatusCode)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    using (StreamWriter writer = new StreamWriter(filename))
                    {
                        writer.Write(contents);
                    }
                }
            }                  
            ReadTheWordsfile(filename);
           
        }

        // read words from file and but it in a list
        public void ReadTheWordsfile(string filename)
        {
            using (StreamReader s = new StreamReader(filename))
            {
                string line = "";
                while ((line = s.ReadLine()) != null)
                {
                    wordslist.Add(line);
                    Console.WriteLine(line);
                }
            }
        }

       private ObservableCollection<string> _words = new ObservableCollection<string>();
        public ObservableCollection<string> Words
        {
            get
            {
                return _words;
            }
        }

        // make the collection to get the word
        public async Task MakeCollection()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await GetWords();
                _words.Clear();
                foreach (var word in wordslist)
                {
                    _words.Add(word);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(" Error in loading words ", ex.Message, "OK");
            }
            finally { IsBusy = false; }
        }

        // get random word from the list
        public string GetRandomWord()
        {
            int which = random.Next(0, wordslist.Count);
            return wordslist[which];

        }
    }
}