using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


public class InstagramFacebookPost
{
    private readonly string accessToken;
    private readonly string igUserId;
    private readonly string imageUrl;
    private readonly string caption;
    private readonly string pageId;

    public InstagramFacebookPost(string caption,string imgUrl)
    {
        accessToken = "EAAFLqS6PKXsBO8Hmbe9sgSUQWqnTjnqMTVz7vfUGfcPVHSiGbZC0ZB0R56pHO9TSszyqnqP9e7w8qV7CZCZAuySbUZCjYZAKNbvFoMlsonrkYe87AM4pLzLsWQZA5o9LlHtOmZAktRKbViNW6menBTh2aAouio8FU80pn1oYLuJes7ihMQDLxzbevvOaJHkn92uq";
        igUserId = "17841461169630863";
        imageUrl = imgUrl;
        this.caption = caption;
        pageId = "122106642020003010";
    }

    public async Task UploadMedia()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("Please enter the Facebook token.");
            return;
        }

        var instagramUploadUrl = $"https://graph.facebook.com/v18.0/{igUserId}/media";
        var payload = new
        {
            image_url = imageUrl,
            caption = caption,
            access_token = accessToken
        };

        try
        {
            var igResponse = await PostRequest(instagramUploadUrl, payload);
            if (igResponse.Contains("id"))
            {
                var creationId = JsonConvert.DeserializeObject<dynamic>(igResponse).id;
                await PublishMedia(creationId.ToString());
            }
            else
            {
                Console.WriteLine("Instagram media upload failed: " + igResponse);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        await PostToFacebook();
    }

    private async Task<string> PostRequest(string url, object payload)
    {
        using (var client = new HttpClient())
        {
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var response = await client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
            return await response.Content.ReadAsStringAsync();
        }
    }

    private async Task PublishMedia(string creationId)
    {
        var publishUrl = $"https://graph.facebook.com/v18.0/{igUserId}/media_publish";
        var payload = new
        {
            creation_id = creationId,
            access_token = accessToken
        };

        try
        {
            var response = await PostRequest(publishUrl, payload);
            var data = JsonConvert.DeserializeObject<dynamic>(response);
            if (!data.ContainsKey("error"))
            {
                Console.WriteLine("Media published successfully.");
            }
            else
            {
                throw new Exception("Publishing failed: " + data.error.message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private async Task PostToFacebook()
    {
        if (string.IsNullOrEmpty(pageId))
        {
            Console.WriteLine("Facebook Page ID is required");
            return;
        }

        var fbUrl = $"https://graph.facebook.com/v18.0/{pageId}/feed?message={caption}&link={imageUrl}&access_token={accessToken}";

        try
        {
            var fbResponse = await PostRequest(fbUrl, null);
            var fbData = JsonConvert.DeserializeObject<dynamic>(fbResponse);
            if (fbData.ContainsKey("id"))
            {
                Console.WriteLine($"Image published to Facebook successfully with post ID: {fbData.id}");
            }
            else
            {
                Console.WriteLine("Facebook post failed: " + fbResponse);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
