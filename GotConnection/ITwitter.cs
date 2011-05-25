namespace GotConnection
{
    public interface ITwitter
    {
        /// <summary>
        /// Timeline with the specified username and specified options
        /// </summary>
        /// <param name="username"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        string TimeLine(string username, object options = null);
    }
}
