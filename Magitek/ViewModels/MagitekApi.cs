using Clio.Utilities.Collections;
using Magitek.Commands;
using Magitek.Models.MagitekApi;
using Magitek.Utilities;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Input;

namespace Magitek.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MagitekApi
    {
        private static MagitekApi _instance;
        private readonly HttpClient _webClient = new HttpClient();
        private const string GithubAddress = "https://api.github.com";
        private const string VersionUrl = "https://github.com/MagitekRB/MagitekRoutine/releases/latest/download/Version.txt";

        public static MagitekApi Instance => _instance ?? (_instance = new MagitekApi());
        public bool SpinnerVisible { get; set; } = false;
        public AsyncObservableCollection<MagitekNews> NewsList { get; set; }
        public MagitekVersion MagitekVersion { get; set; }
        public ICommand RefreshNewsList => new DelegateCommand(UpdateNews);

        private class Release
        {
            public string tag_name { get; set; }
            public string name { get; set; }
            public string body { get; set; }
            public DateTime created_at { get; set; }
            public bool prerelease { get; set; }
        }

        public MagitekApi()
        {
            SpinnerVisible = true;
            try
            {
                NewsList = new AsyncObservableCollection<MagitekNews>();
                UpdateVersion();
                UpdateNews();
            }
            catch (Exception)
            { }
            SpinnerVisible = false;
        }

        private async void UpdateVersion()
        {
            var local = "UNKNOWN";
            var distant = "UNKNOWN";

            try
            {
                local = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $@"Routines\Magitek\Version.txt"));
            }
            catch
            {
                Logger.Error("Can't read local Magitek version. Please reinstall it");
            }

            try
            {
                distant = await _webClient.GetStringAsync(VersionUrl);
            }
            catch
            {
                Logger.Error("Can't read distant Magitek version. Please reinstall it");
            }
            MagitekVersion = new MagitekVersion()
            {
                LocalVersion = local,
                DistantVersion = distant
            };
        }

        private async void UpdateNews()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(GithubAddress);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Anything");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.GetFromJsonAsync<List<Release>>("repos/MagitekRB/MagitekRoutine/releases");

                    if (response == null)
                        return;

                    response.ForEach(x =>
                    {
                        if (x?.prerelease == true)
                            return;

                        // Process the body to remove commit hashes and PR links
                        var bodyLines = x.body.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        var filteredBodyLines = bodyLines
                            .Where(line => line.StartsWith("- "))
                            .Select(line =>
                            {
                                var parts = line.Split(':');
                                return parts.Length > 1 ? parts[1].Split('[')[0].Trim() : line;
                            });

                        var filteredBody = string.Join("\n", filteredBodyLines);

                        NewsList.Add(new MagitekNews
                        {
                            Created = x.created_at.ToString("d"),
                            Title = x.name,
                            Message = filteredBody
                        });
                    });
                };
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            SpinnerVisible = false;
        }
    }
}
