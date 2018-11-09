using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GildedRose.HttpClient.Exceptions;

namespace GildedRose.HttpClient
{
    public class ApiRequestHandler : IProxy
    {
        public static readonly System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();

        public async Task<T> Get<T>(string url)
        {
            try
            {
                var response = await Client.GetAsync(url);
                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    var serializedData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(serializedData);
                }
                else if ((int)response.StatusCode == 400)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else if ((int)response.StatusCode == 404)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<object> Get(string url)
        {
            try
            {
                var response = await Client.GetAsync(url);
                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else if ((int)response.StatusCode == 400)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else if ((int)response.StatusCode == 404)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<TReturn> Post<TInput, TReturn>(string url, TInput content)
        {
            try
            {
                var serializedContent = JsonConvert.SerializeObject(content);
                var contentBuffer = Encoding.UTF8.GetBytes(serializedContent);
                var contentByteArray = new ByteArrayContent(contentBuffer);
                contentByteArray.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await Client.PostAsync(url, contentByteArray);

                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    var serializedData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TReturn>(serializedData);
                }
                else if ((int)response.StatusCode == 400)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else if ((int)response.StatusCode == 404)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<T> Put<T>(string url, T content)
        {
            try
            {
                var serializedContent = JsonConvert.SerializeObject(content);
                var contentBuffer = Encoding.UTF8.GetBytes(serializedContent);
                var contentByteArray = new ByteArrayContent(contentBuffer);
                contentByteArray.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await Client.PutAsync(url, contentByteArray);

                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    var serializedData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(serializedData);
                }
                else if ((int)response.StatusCode == 400)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else if ((int)response.StatusCode == 404)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<T> Delete<T>(string url)
        {
            try
            {
                var response = await Client.DeleteAsync(url);
                if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
                {
                    var serializedData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(serializedData);
                }
                else if ((int)response.StatusCode == 400)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else if ((int)response.StatusCode == 404)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }
    }
}
