namespace EightTracksPlayer.Communication.Responses
{
    public class ResponseStatusEncoder
    {
        public static ResponseStatusEnum Decode(string statusString)
        {
            switch (statusString)
            {
                case "200 OK":
                    return ResponseStatusEnum.Ok;
                case "301 Moved Permanently":
                    return ResponseStatusEnum.MovedPermanently;
                case "302 Moved":
                    return ResponseStatusEnum.Moved;
                case "401 Unauthorized":
                    return ResponseStatusEnum.Unauthorized;
                case "403 Forbidden":
                    return ResponseStatusEnum.Forbidden;
                case "404 Not Found":
                    return ResponseStatusEnum.NotFound;
                case "422 Unprocessable Entity":
                    return ResponseStatusEnum.UnprocessableEntity;
                case "500 Internal Server Error":
                    return ResponseStatusEnum.InternalServerError;
                default:
                    return ResponseStatusEnum.Unknown;
            }
        }

        public static string Encode(ResponseStatusEnum status)
        {
            switch (status)
            {
                case ResponseStatusEnum.Ok:
                    return "200 OK";
                case ResponseStatusEnum.MovedPermanently:
                    return "301 Moved Permanently";
                case ResponseStatusEnum.Moved:
                    return "302 Moved";
                case ResponseStatusEnum.Unauthorized:
                    return "401 Unauthorized";
                case ResponseStatusEnum.Forbidden:
                    return "403 Forbidden";
                case ResponseStatusEnum.NotFound:
                    return "404 Not Found";
                case ResponseStatusEnum.UnprocessableEntity:
                    return "422 Unprocessable Entity";
                case ResponseStatusEnum.InternalServerError:
                    return "500 Internal Server Error";
                default:
                    return "-1 Unknown";
            }
        }
    }

    public enum ResponseStatusEnum
    {
        Ok = 200,
        MovedPermanently = 301,
        Moved = 302,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        UnprocessableEntity = 422,
        InternalServerError = 500,
        Unknown = -1
    }
}