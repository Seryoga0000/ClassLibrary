using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ClassLibrary
{
    public class SaveToFile : INotifyPropertyChanged, IDisposable
    {
        //public static string CreateFolderPath()
        //{
        //    var t = $@"{AppDomain.CurrentDomain.BaseDirectory}Data" ;
        //    Directory.CreateDirectory(t);
        //    return t;
        //}
        //public string FolderPath { get; private set; }

        //public SaveToFile([NotNull] Stream stream) : base(stream)
        //{
        //    Constructor();
        //}

        //public SaveToFile([NotNull] Stream stream, [NotNull] Encoding encoding) : base(stream, encoding)
        //{
        //    Constructor();
        //}

        //public SaveToFile([NotNull] Stream stream, [NotNull] Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize)
        //{
        //    Constructor();
        //}

        //public SaveToFile([NotNull] string path) : base(path)
        //{
        //    Constructor();
        //}

        //public SaveToFile([NotNull] string path, bool append) : base(path, append)
        //{
        //    Constructor();
        //}

        //public SaveToFile([NotNull] string path, bool append, [NotNull] Encoding encoding) : base(path, append, encoding)
        //{
        //    Constructor();
        //}

        //public SaveToFile([NotNull] string path, bool append, [NotNull] Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
        //{
        //    Constructor();
        //}

        //private void Constructor()
        //{
        //    FolderPath = ((FileStream) BaseStream).Name;

        //}

        private const string fname = "Data";
        private const string continuation = "_next";
        private long absoluteLineCounter;
        private bool autoflash = true;

        //public SaveToFile(IFileCore core)
        //{
        //    core.DataUpdate += Core_DataUpdate;
        //}

        //private void Core_DataUpdate(object sender, CoreClass.DataUpdateArgs e)
        //{
        //    List<Measurument.DataPoints> list = e.UpdateDataList;
        //    if (list == null || list.Count == 0) return;
        //    try
        //    {
        //        int count = list.Count;
        //        //int listCount = list.Count;
        //        for (int i = 0; i < count; i++)
        //        {
        //            WriteToFile($"{list[i].Time}_{list[i].DoubleValue}_{list[i].StringValue}");
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        private StreamWriter fstream;
        private string fullPathFile = "";
        private long lineCounter;
        private long lineLimit = 100000;

        /// <summary>Содержит путь к папке с программой.</summary>
        public string AppFolder { get; private set; } = "";

        /// <summary>Содержит относительный путь к файлу.</summary>
        public string RelatePathFile { get; private set; } = "";

        /// <summary>Содержит относительный путь к папке с файлом.</summary>
        public string RelatepathFolder { get; private set; } = "";

        /// <summary>Содержит полный путь к папке с файлом.</summary>
        public string FullpathFolder { get; private set; } = "";

        /// <summary>Содержит полный путь к файлу.</summary>
        public string FullPathFile
        {
            get => fullPathFile;
            private set
            {
                fullPathFile = value;
                OnPropertyChanged(nameof(FullPathFile));
            }
        }

        public string FileName { get; private set; } = "";
        public bool IsOpened { get; private set; }

        /// <summary>Содержит количество строк в файле.</summary>
        public long LineCounter
        {
            get => lineCounter;
            private set
            {
                lineCounter = value;
                OnPropertyChanged(nameof(LineCounter));
            }
        }

        public int FileCounter { get; private set; } = 1;

        public long AbsoluteLineCounter
        {
            get => absoluteLineCounter;
            private set
            {
                absoluteLineCounter = value;
                OnPropertyChanged(nameof(AbsoluteLineCounter));
            }
        }

        public long LineLimit
        {
            get => lineLimit;
            set
            {
                lineLimit = value;
                OnPropertyChanged(nameof(LineLimit));
            }
        }

        public bool Autoflash
        {
            get => autoflash;
            set
            {
                autoflash = value;
                if (fstream != null) fstream.AutoFlush = value;
            }
        }

        /// <summary>
        ///     Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых
        ///     ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            fstream?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Создает папку с названием в папке с программой.</summary>
        public static string CreateFolder(string folderName)
        {
            var t = $@"{AppDomain.CurrentDomain.BaseDirectory}{folderName}";
            Directory.CreateDirectory(t);
            return t;
        }

        /// <summary>Вычисляет путь к папке с программой.</summary>
        public static string GetAppFolder()
        {
            var t = $@"{AppDomain.CurrentDomain.BaseDirectory}";
            return t;
        }

        public void Open()
        {
            absoluteLineCounter = 0;
            FileCounter = 0;
            OpenSub();
        }

        public void Open(string prename)
        {
            absoluteLineCounter = 0;
            FileCounter = 0;
            OpenSub(prename);
        }

        public void Open(string prename, string aftername)
        {
            absoluteLineCounter = 0;
            FileCounter = 0;
            OpenSub(prename, aftername);
        }

        private void OpenSub(string prename = "", string aftername = "")
        {
            CreateFolder();
            CreateNewFilePath(prename, aftername);

            fstream?.Close();
            fstream = new StreamWriter(FullPathFile, true, Encoding.Default);

            LineCounter = 0;
            IsOpened = true;

            Autoflash = true;
        }


        private void CreateFolder()
        {
            string t = CreateFolder(fname);
            FullpathFolder = t;
            AppFolder = GetAppFolder();
            RelatepathFolder = Path.Combine("", fname);
        }

        private void CreateNewFilePath(string prename = "", string aftername = "")
        {
            string t;
            //t = $@"{FullpathFolder}{DateTime.Now:MM.dd.yyyy HH_mm_ss}.txt";
            t = Path.Combine(FullpathFolder,
                $"{prename}{DateTime.Now:MM.dd.yyyy HH_mm_ss}{aftername}.txt");
            FullPathFile = t;
            FileName = Path.GetFileName(t);
            RelatePathFile = Path.Combine(fname, FileName ?? throw new InvalidOperationException());
            
        }

        public string CloneFile(string fullPath = "", string targetFolder = "", string fileName = "Clone.txt")
        {
            try
            {
                string fp = fullPath == "" ? FullPathFile : fullPath;
                string tf = targetFolder == "" ? FullpathFolder : targetFolder;
                string new_fp = tf + "\\" + fileName;
                File.Copy(fp, new_fp, true);

                return new_fp;
            }
            catch (Exception ex)
            {
                return "Err";
            }
        }

        /// <summary>Запись строки в файл. Если количество строк превышает установленый лимит создается новый файл.</summary>
        public void WriteToFile(string PData)
        {
            if (!IsOpened) return;
            fstream.WriteLine(PData);
            LineCounter++;
            AbsoluteLineCounter++;
            if (LineCounter > LineLimit)
            {
                FileCounter++;
                OpenSub("", continuation);
            }

            //_linecounter++;
            //if (counterOn) { fstream.WriteLine(_linecounter + "_" + PData); }
            //else { fstream.WriteLine(PData); }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}