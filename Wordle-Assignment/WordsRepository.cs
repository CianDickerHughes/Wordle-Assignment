using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wordle_Assignment
{
    public class WordsRepository
    {
        private bool isBusy;
        private Random random;
        private List<string> wordslist;
        HttpClient httpClient;
        private int _wordsCount;


        public WordsRepository()
        {
            wordslist = new List<string>();
            random = new Random();
            // fillWordsList();
            _wordsCount = wordslist.Count;
            httpClient = new();
            //GetWordCommand = new Command(async () => await MakeCollection());
            //GoToDetailsCommand = new Command<Words>(async (monkey) => await GoToDetails(monkey));
        }


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
                    //wordslist = JsonSerializer.Deserialize<List<Words>>(contents);
                    //Save cntents t a fie
                    using (StreamWriter writer = new StreamWriter(filename))
                    {
                        writer.Write(contents);
                    }
                }
            }                  
            ReadTheWordsfile(filename);
           
        }

        public void ReadTheWordsfile(string filename)
        {
            using (StreamReader s = new StreamReader(filename))
            {
                string line = "";
                while ((line = s.ReadLine()) != null)
                {
                    wordslist.Add(line);
                    Console.WriteLine(line);
                    _wordsCount++;
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

        public List<string> theList
        {
            get { return wordslist; }
            set { wordslist = value; }
        }

        public Command GetWordCommand { get; }

        public Command GoToDetailsCommand { get; }

        public string GiveRandomWord()
        {
            int which = random.Next(0, wordslist.Count);
            return wordslist[which];

        }

    }
}





        /* public List<Words> theList
         {
             get { return _list; }
             set { _list = value; }
         }

         private int _wordsCount;

         public int wordsCount
         {
             get { return _wordsCount; }
             set { _wordsCount = value; }
         }

         public async void fillWordsList() 
         {
             String line;
             Words aWords = new Words();
             try 
             { 
                 using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
                 using StreamReader reader = new StreamReader(fileStream);
                 while ((line = reader.ReadLine()) != null) 
                 {
                     aWords = new Words();
                     aWords.theWords = line;
                     _list.Add(aWords);
                 } // end loop
             }
             catch (Exception ex) 
             { 
                 _wordsCount = -1;
                 Words error = new Words();
                 error.theWords = ex.ToString();
                 _list.Add(error);
             }// end catch

         }// end fill list


 */
