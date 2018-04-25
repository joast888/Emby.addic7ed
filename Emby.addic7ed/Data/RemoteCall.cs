﻿using System.IO;
using System.Threading.Tasks;
using Emby.addic7ed.Model;
using MediaBrowser.Common.Net;

namespace Emby.addic7ed.Data
{
    internal class RemoteCall
    {
        private readonly IHttpClient _httpClient;

        private const string ServiceUrl = "http://www.addic7ed.com";

        private HttpRequestOptions BaseRequestOptions => new HttpRequestOptions
        {
            UserAgent =
                "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 65.0.3325.181 Safari / 537.36",
            Referer = ServiceUrl
        };

        public RemoteCall(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRemoteSeries()
        {
            var httpOpt = BaseRequestOptions;
            httpOpt.Url = ServiceUrl + "/ajax_getShows.php";

            using (HttpResponseInfo httpResponseInfo = await _httpClient.GetResponse(httpOpt).ConfigureAwait(false))
            {
                using (StreamReader streamReader = new StreamReader(httpResponseInfo.Content))
                {
                    return await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<string> GetRemoteEpisodes(long showId, long season)
        {
            var httpOpt = BaseRequestOptions;
            httpOpt.Url = ServiceUrl + string.Format("/ajax_getEpisodes.php?showID={0}&season={1}", showId, season);

            using (HttpResponseInfo httpResponseInfo = await _httpClient.GetResponse(httpOpt).ConfigureAwait(false))
            {
                using (StreamReader streamReader = new StreamReader(httpResponseInfo.Content))
                {
                    return await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<string> GetRemoteEpisodeDetail(RemoteEpisode episode)
        {
            var httpOpt = BaseRequestOptions;
            httpOpt.Url = ServiceUrl + string.Format("/re_episode.php?ep={0}", episode.RemoteId);

            using (HttpResponseInfo httpResponseInfo = await _httpClient.GetResponse(httpOpt).ConfigureAwait(false))
            {
                using (StreamReader streamReader = new StreamReader(httpResponseInfo.Content))
                {
                    return await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<Stream> GetSubtitles(string url)
        {
            var httpOpt = BaseRequestOptions;
            httpOpt.Url = ServiceUrl + url;

            using (HttpResponseInfo httpResponseInfo = await _httpClient.GetResponse(httpOpt).ConfigureAwait(false))
            {
                MemoryStream ms = new MemoryStream();
                await httpResponseInfo.Content.CopyToAsync(ms).ConfigureAwait(false);
                return ms;
            }
        }
    }
}